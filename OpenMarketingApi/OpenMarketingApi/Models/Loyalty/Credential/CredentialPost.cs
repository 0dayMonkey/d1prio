namespace OpenMarketingApi.Models.Loyalty.Credential
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class CredentialPost
    {
        /// <summary>
        /// All the credentials linked to the same physical support
        /// </summary>
        /// <value>All the credentials linked to the same physical support</value>
        [DataMember(Name = "credentials")]
        public List<LoyaltyCredentialData> Credentials { get; set; }

        /// <summary>
        /// mandatory for Magnetic type, ignored elsewhere
        /// </summary>
        /// <value>mandatory for Magnetic type, ignored elsewhere</value>
        [DataMember(Name = "issuingSiteId")]
        public decimal IssuingSiteId { get; set; }

        /// <summary>
        /// Gets or Sets BeginValidityTimestamp
        /// </summary>
        [DataMember(Name = "beginValidityTimestamp")]
        public DateTime BeginValidityTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets EndValidityTimestamp
        /// </summary>
        [DataMember(Name = "endValidityTimestamp")]
        public DateTime EndValidityTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets SupportType
        /// </summary>

        [DataMember(Name = "supportType")]
        public List<SupportType> SupportType { get; set; }

        /// <summary>
        /// Gets or Sets SupportId
        /// </summary>

        [DataMember(Name = "supportId")]
        public string? SupportId { get; set; }

        /// <summary>
        /// Gets or Sets lastUpdatedTimestamp
        /// </summary>
        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
