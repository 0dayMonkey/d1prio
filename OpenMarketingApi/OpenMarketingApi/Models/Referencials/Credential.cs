namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Credential
    {
        /// <summary>
        /// Gets or Sets CredentialId
        /// </summary>
        [DataMember(Name="credentialId")]
        public decimal CredentialId { get; set; }

        /// <summary>
        /// Gets or Sets CredentialType
        /// </summary>
        [DataMember(Name="credentialType")]
        public string CredentialType { get; set; }

        /// <summary>
        /// Gets or Sets CredentialCode
        /// </summary>
        [DataMember(Name="credentialCode")]
        public string CredentialCode { get; set; }

        /// <summary>
        /// Gets or Sets DisplayCode
        /// </summary>
        [DataMember(Name="displayCode")]
        public string DisplayCode { get; set; }

        /// <summary>
        /// Gets or Sets IssuingSiteId
        /// </summary>
        [DataMember(Name="issuingSiteId")]
        public decimal? IssuingSiteId { get; set; }

        /// <summary>
        /// Gets or Sets IsLocked
        /// </summary>
        [DataMember(Name="isLocked")]
        public bool IsLocked { get; set; }

        /// <summary>
        /// Gets or Sets LockReason
        /// </summary>
        [DataMember(Name="lockReason")]
        public List<string> LockReason { get; set; }

        /// <summary>
        /// Gets or Sets IsPinLocked
        /// </summary>

        [DataMember(Name = "isPinLocked")]
        public bool? IsPinLocked { get; set; }

        /// <summary>
        /// Gets or Sets CreationTimestamp
        /// </summary>

        [DataMember(Name="creationTimestamp")]
        public DateTime? CreationTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets BeginValidityTimestamp
        /// </summary>

        [DataMember(Name="beginValidityTimestamp")]
        public DateTime? BeginValidityTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets EndValidityTimestamp
        /// </summary>

        [DataMember(Name="endValidityTimestamp")]
        public DateTime? EndValidityTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets LockTimestamp
        /// </summary>

        [DataMember(Name="lockTimestamp")]
        public DateTime? LockTimestamp { get; set; }
    }
}
