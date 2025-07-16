namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlayerMaritalStatus
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [MaxLength(1)]
        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets ShortLabel
        /// </summary>

        [MaxLength(15)]
        [DataMember(Name="shortLabel")]
        public string ShortLabel { get; set; }

        /// <summary>
        /// Gets or Sets LongLabel
        /// </summary>

        [MaxLength(30)]
        [DataMember(Name="longLabel")]
        public string LongLabel { get; set; }
    }
}
