using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Models.Referencials.IdentityDocument
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class IdDocument
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required]
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets DocumentNumber
        /// </summary>
        [Required]
        [DataMember(Name = "documentNumber")]
        public string DocumentNumber { get; set; }

        /// <summary>
        /// Gets or Sets DocumentType
        /// </summary>
        [Required]
        [DataMember(Name = "documentType")]
        public IdDescriptionType DocumentType { get; set; }

        /// <summary>
        /// Gets or Sets IssueDate
        /// </summary>

        [DataMember(Name = "issueDate")]
        public DateTime? IssueDate { get; set; }

        /// <summary>
        /// Gets or Sets ExpiryDate
        /// </summary>

        [DataMember(Name = "expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Gets or Sets IssuingCountry
        /// </summary>
        [Required]
        [DataMember(Name = "issuingCountry")]
        public AddressCountry IssuingCountry { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdatedTimestamp
        /// </summary>

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
