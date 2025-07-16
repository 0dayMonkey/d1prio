namespace OpenMarketingApi.Models.Referencials.IdentityDocument
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class IdDocumentType
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name="id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or Sets ShortLabel
        /// </summary>
        [DataMember(Name="shortLabel")]
        public string ShortLabel { get; set; }

        /// <summary>
        /// Gets or Sets LongLabel
        /// </summary>
        [DataMember(Name="longLabel")]
        public string LongLabel { get; set; }

        /// <summary>
        /// Gets or Sets YearsValidity
        /// </summary>
        [DataMember(Name="yearsValidity")]
        public int? YearsValidity { get; set; }

        /// <summary>
        /// Gets or Sets Official
        /// </summary>

        [DataMember(Name="official")]
        public bool? Official { get; set; }

        /// <summary>
        /// Gets or Sets Active
        /// </summary>

        [DataMember(Name="active")]
        public bool? Active { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdatedTimestamp
        /// </summary>

        [DataMember(Name="lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }

        [DataMember(Name = "countrySpecificities")]
        public List<IdentityDocumentTypeByCountry>? CountrySpecificities { get; set; }
    }
}
