namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class Comment
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Text
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or Sets ExpiryDate
        /// </summary>

        [DataMember(Name="expiryDate")]
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Gets or Sets IsReadMandatory
        /// </summary>

        [DataMember(Name="isReadMandatory")]
        public bool? IsReadMandatory { get; set; }

        /// <summary>
        /// Gets or Sets CreationTimestamp
        /// </summary>

        [DataMember(Name="creationTimestamp")]
        public DateTime? CreationTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>
        [DataMember(Name="creationUserId")]
        public string CreationUserId { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdatedTimestamp
        /// </summary>

        [DataMember(Name="lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdatedUserId
        /// </summary>

        [DataMember(Name="lastUpdatedUserId")]
        public string LastUpdatedUserId { get; set; }
    }
}
