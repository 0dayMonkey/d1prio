using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Referencials.Address
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class CountryPUT : IServiceAction
    {
        /// <summary>
        /// Gets or Sets ShortLabel
        /// </summary>
        [Required]
        [DataMember(Name = "shortLabel")]
        public string ShortLabel { get; set; }

        /// <summary>
        /// Gets or Sets LongLabel
        /// </summary>
        [Required]
        [DataMember(Name = "longLabel")]
        public string LongLabel { get; set; }

        /// <summary>
        /// ISO 639-1 language code
        /// </summary>
        /// <value>ISO 639-1 language code</value>
        [DataMember(Name = "languageId")]
        public string? LanguageId { get; set; }

        /// <summary>
        /// ISO 3166-1 alpha-2
        /// </summary>
        /// <value>ISO 3166-1 alpha-2</value>
        [Required]
        [DataMember(Name = "isoAlpha2")]
        public string IsoAlpha2 { get; set; }

        /// <summary>
        /// ISO 3166-1 numeric
        /// </summary>
        /// <value>ISO 3166-1 numeric</value>
        [Required]
        [DataMember(Name = "isoNumeric")]
        public decimal IsoNumeric { get; set; }

        /// <summary>
        /// Gets or Sets NationalityLabel
        /// </summary>
        [Required]
        [DataMember(Name = "nationalityLabel")]
        public string NationalityLabel { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }
		



    }
}
