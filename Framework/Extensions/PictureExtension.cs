using System.Drawing;
using System.Drawing.Imaging;

namespace Framework.Extensions
{
    public static class PictureExtension
    {
        public static bool IsImageValid(this byte[] imageBytes)
        {
            if (imageBytes == null || imageBytes.Length == 0)
            {
                return false;
            }

            try
            {
                using (var stream = new MemoryStream(imageBytes))
                {
                    using (var image = Image.FromStream(stream))
                    {
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
