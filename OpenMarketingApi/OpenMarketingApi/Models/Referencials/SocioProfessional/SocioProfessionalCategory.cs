namespace OpenMarketingApi.Models.Referencials.SocioProfessional
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class SocioProfessionalCategory
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

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
        /// Gets or Sets LastUpdatedTimestamp
        /// </summary>

        [DataMember(Name = "lastUpdatedTimestamp")]
        public string LastUpdatedTimestamp { get; set; }
    }
}
