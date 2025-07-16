using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Referencials.Address
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class CityPUT : IServiceAction
    { 
        /// <summary>
        /// Gets or Sets ShortLabel
        /// </summary>
        [Required]
        [DataMember(Name="shortLabel")]
        public string ShortLabel { get; set; }

        /// <summary>
        /// Gets or Sets LongLabel
        /// </summary>
        [Required]
        [DataMember(Name="longLabel")]
        public string LongLabel { get; set; }

        /// <summary>
        /// Gets or Sets PostalCodes
        /// </summary>
        [Required]
        [DataMember(Name="postalCodes")]
        public List<string> PostalCodes { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }

        [DataMember(Name = "addressLevel3Id")]
        public string? AddressLevel3Id { get; set; }
    }
}
