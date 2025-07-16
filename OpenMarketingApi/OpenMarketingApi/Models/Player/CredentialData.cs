namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class CredentialData
    { 
        /// <summary>
        /// Gets or Sets CredentialType
        /// </summary>
        [Required]

        [DataMember(Name="credentialType")]
        public CredentialType CredentialType { get; set; }

        /// <summary>
        /// Gets or Sets CredentialCode
        /// </summary>
        [DataMember(Name="credentialCode")]
        public string? CredentialCode { get; set; }
    }
}
