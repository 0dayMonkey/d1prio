namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Location
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [Required]
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets SiteId
        /// </summary>

        [DataMember(Name="siteId")]
        public int? SiteId { get; set; }

        /// <summary>
        /// Gets or Sets LocationType
        /// </summary>

        [DataMember(Name="locationType")]
        public LocationTypeEnum? LocationType { get; set; }
    }
}
