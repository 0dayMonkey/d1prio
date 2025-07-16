namespace OpenMarketingApi.Models.Referencials.Address
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class AddressCountry
    { 
        /// <summary>
        /// ISO 3166-1 alpha-3
        /// </summary>
        /// <value>ISO 3166-1 alpha-3</value>
        [DataMember(Name="id")]
        public string Id { get; set; }

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
    }
}
