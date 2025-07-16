namespace OpenMarketingApi.Models.Referencials.Address
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Country
    {
        /// <summary>
        /// ISO 3166-1 alpha-3
        /// </summary>
        /// <value>ISO 3166-1 alpha-3</value>

        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets ShortLabel
        /// </summary>

        [DataMember(Name = "shortLabel")]
        public string ShortLabel { get; set; }

        /// <summary>
        /// Gets or Sets LongLabel
        /// </summary>

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


        [DataMember(Name = "isoAlpha2")]
        public string IsoAlpha2 { get; set; }

        /// <summary>
        /// ISO 3166-1 numeric
        /// </summary>
        /// <value>ISO 3166-1 numeric</value>

        [DataMember(Name = "isoNumeric")]
        public decimal? IsoNumeric { get; set; }

        /// <summary>
        /// Gets or Sets NationalityLabel
        /// </summary>


        [DataMember(Name = "nationalityLabel")]
        public string NationalityLabel { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime LastUpdatedTimestamp { get; set; }
    }
}
