namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Nationality
    { 
        /// <summary>
        /// ISO 3166-1 alpha-3
        /// </summary>
        /// <value>ISO 3166-1 alpha-3</value>

        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Label
        /// </summary>
        [DataMember(Name="label")]
        public string Label { get; set; }
    }
}
