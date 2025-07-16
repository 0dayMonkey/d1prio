using ApiTools.Converters;
using OpenMarketingApi.Convertors;
using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Loyalty.Point
{
    [DataContract]
    [JsonConverter(typeof(PointTransactionConverter))]
    public abstract class PointTransactionBasePOST : IServiceAction
    {
        [Required]
        [DataMember(Name = "gamingDay")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime GamingDay { get; set; }

        [Required]
        [DataMember(Name = "siteId")]
        public int SiteId { get; set; }

        [Required]
        [DataMember(Name = "locationId")]
        public string LocationId { get; set; }

        [Required]
        [DataMember(Name = "locationType")]
        public LocationTypeEnum LocationType { get; set; }

        [DataMember(Name = "creationUserCode")]
        public string? CreationUserCode { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
