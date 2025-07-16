namespace OpenMarketingApi.Models.Referencials.Address
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class CityResponse
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required]

        [MaxLength(21)]
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets ShortLabel
        /// </summary>
        [Required]

        [MaxLength(15)]
        [DataMember(Name="shortLabel")]
        public string ShortLabel { get; set; }

        /// <summary>
        /// Gets or Sets LongLabel
        /// </summary>
        [Required]

        [MaxLength(30)]
        [DataMember(Name="longLabel")]
        public string LongLabel { get; set; }

        /// <summary>
        /// Gets or Sets PostalCodes
        /// </summary>

        [DataMember(Name="postalCodes")]
        public List<string>? PostalCodes { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime LastUpdatedTimestamp { get; set; }

        public AddressLevelRef? AddressLevel1 { get; set; }
        public AddressLevelRef? AddressLevel2 { get; set; }
        public AddressLevelRef? AddressLevel3 { get; set; }
    }
}
