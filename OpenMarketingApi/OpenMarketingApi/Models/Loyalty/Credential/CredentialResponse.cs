namespace OpenMarketingApi.Models.Loyalty.Credential
{
    public partial class CredentialResponse
    {
        /// <summary>
        /// Gets or Sets CredentialId
        /// </summary>
        [DataMember(Name = "credentialId")]
        public decimal CredentialId { get; set; }

        /// <summary>
        /// Gets or Sets CredentialType
        /// </summary>
        [DataMember(Name = "credentialType")]
        public string CredentialType { get; set; }

        /// <summary>
        /// Gets or Sets CredentialCode
        /// </summary>
        [DataMember(Name = "credentialCode")]
        public string CredentialCode { get; set; }

        /// <summary>
        /// mandatory for Magnetic type, ignored elsewhere
        /// </summary>
        /// <value>mandatory for Magnetic type, ignored elsewhere</value>
        [DataMember(Name = "issuingSiteId")]
        public int? IssuingSiteId { get; set; }

        /// <summary>
        /// Gets or Sets BeginValidityTimeStamp
        /// </summary>
        [DataMember(Name = "beginValidityTimestamp")]
        public DateTime? BeginValidityTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets EndValidityTimestamp
        /// </summary>
        [DataMember(Name = "endValidityTimestamp")]
        public DateTime? EndValidityTimestamp { get; set; }

        [DataMember(Name = "isLocked")]
        public bool IsLocked { get; set; }


        [DataMember(Name = "isPinLocked")]
        public bool IsPinLocked { get; set; }

        [DataMember(Name = "lockTimestamp")]
        public DateTime? LockTimestamp { get; set; }

        [DataMember(Name = "lockReasons")]
        public List<string> LockReasons { get; set; }

        [DataMember(Name = "creationTimestamp")]
        public DateTime? CreationTimestamp { get; set; }
    }
}