using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Referencials.Occupation
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class OccupationPOST : IServiceAction
    {
        [DataMember(Name = "id")]
        public string? Id { get; set; }
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

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }



    }
}
