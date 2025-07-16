using Galaxis.Configuration.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Galaxis.Configuration.Transformations
{
    public class TransformationManager
    {
        private enum KeySize
        {
            //Key size of 128 bits
            Key128 = 16,
            //Key size of 192 bits
            Key192 = 24,
            //Key size of 256 bits
            Key256 = 32
        }

        private const KeySize UsedKeySize = KeySize.Key128;

        private readonly ILogger<TransformationManager> _logger;
        private readonly IConfiguration _configuration;

        public TransformationManager(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory?.CreateLogger<TransformationManager>();
            _configuration = configuration;
        }

        public TransformationManager(ILoggerFactory loggerFactory): this(loggerFactory, InMemoryTransformationConfiguration.CreateConfiguration())
        {
        }


        // Performs the transformation
        public string Apply(string stringToEncrypt)
        {
            if (string.IsNullOrEmpty(stringToEncrypt))
                return null;

            try
            {
                using (SymmetricAlgorithm aesAlgorithm = Aes.Create("AesManaged"))
                {
                    InitializeKeyAndIV(aesAlgorithm);

                    // Convert strings into byte arrays.
                    byte[] stringData = Encoding.Default.GetBytes(stringToEncrypt);

                    // Generate encryptor from the existing key bytes and initialization 
                    // vector. Key size will be defined based on the number of the key 
                    // bytes.
                    ICryptoTransform encryptor = aesAlgorithm.CreateEncryptor();

                    // Define memory stream which will be used to hold encrypted data.
                    var memoryStream = new MemoryStream();

                    // Define cryptographic stream (always use Write mode for encryption).
                    var cryptoStream = new CryptoStream(memoryStream,
                                                                    encryptor,
                                                                    CryptoStreamMode.Write);

                    // Start encrypting.
                    cryptoStream.Write(stringData,
                                        0,
                                        stringData.Length);

                    // Finish encrypting.
                    cryptoStream.FlushFinalBlock();

                    // Convert our encrypted data from a memory stream into a byte array.
                    byte[] cipherTextBytes = memoryStream.ToArray();

                    // Close both streams.
                    memoryStream.Close();
                    cryptoStream.Close();

                    // Convert encrypted data into a base64-encoded string.
                    string cipherText = Convert.ToBase64String(cipherTextBytes);

                    // Return encrypted string.
                    return cipherText;
                }
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error on transforming data");
                return null;
            }
        }

        // Performs the AES decryption.
        public string Undo(string stringToDecrypt)
        {
            if (string.IsNullOrEmpty(stringToDecrypt))
            {
                return null;
            }

            try
            {
                using (SymmetricAlgorithm aesAlgorithm = Aes.Create("AesManaged"))

                {
                    InitializeKeyAndIV(aesAlgorithm);

                    byte[] cipherTextBytes = Convert.FromBase64String(stringToDecrypt);

                    using (var memoryStream = new MemoryStream(cipherTextBytes))

                    using (var cryptoStream = new CryptoStream(memoryStream, aesAlgorithm.CreateDecryptor(), CryptoStreamMode.Read))

                    using (var reader = new StreamReader(cryptoStream, Encoding.UTF8))

                    {

                        string plainText = reader.ReadToEnd();

                        return plainText;

                    }

                }
            }
            catch (Exception e)
            {
                _logger?.LogError(e, "Error on undoing transformation data");
                return stringToDecrypt;
            }
        }

        private void InitializeKeyAndIV(SymmetricAlgorithm algo)
        {
            byte[] salt = Encoding.Default.GetBytes(_configuration["TRANSFORM:S"]);
            var saltedKey = new Rfc2898DeriveBytes(_configuration["TRANSFORM:P"], salt, 3);
            algo.Key = saltedKey.GetBytes((int)UsedKeySize);
            algo.IV = saltedKey.GetBytes(algo.BlockSize / 8);
        }
    }
}
