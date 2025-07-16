namespace OpenMarketingApi.Models.Referencials.IdentityDocument
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class IdDescriptionType
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
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
