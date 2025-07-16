using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Credentials
{
    public partial class CredentialPOST : IServiceAction
    {
        /// <summary>
        /// All the credentials linked to the same physical support
        /// </summary>
        /// <value>All the credentials linked to the same physical support</value>
        [DataMember(Name = "credentials")]
        public List<CredentialData> Credentials { get; set; }

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
