using ApiTools.Converters;
using OpenMarketingApi.Models.Loyalty.Transaction;

namespace OpenMarketingApi.Models.Loyalty.Point
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PointTransaction
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets GamingDay
        /// </summary>

        [DataMember(Name = "gamingDay")]
        [JsonConverter(typeof(DateOnlyConverter))]
        public DateTime GamingDay { get; set; }

        /// <summary>
        /// Gets or Sets Tender
        /// </summary>

        [DataMember(Name = "tender")]
        public TransactionTender Tender { get; set; }

        /// <summary>
        /// Gets or Sets Quantity
        /// </summary>

        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>

        [DataMember(Name = "type")]
        public TransactionType Type { get; set; }

        /// <summary>
        /// Gets or Sets PointAmont
        /// </summary>

        [DataMember(Name = "pointAmount")]
        public double PointAmount { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>

        [DataMember(Name = "status")]
        public TransactionStatus Status { get; set; }

        /// <summary>
        /// Gets or Sets Location
        /// </summary>

        [DataMember(Name = "location")]
        public Location? Location { get; set; }

        /// <summary>
        /// Gets or Sets CreationTimestamp
        /// </summary>

        [DataMember(Name = "creationTimestamp")]
        public DateTime? CreationTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets UserId
        /// </summary>

        [MaxLength(15)]
        [DataMember(Name = "creationUserId")]
        public string? CreationUserId { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdatedTimestamp
        /// </summary>

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }

        /// <summary>
        /// Gets or Sets LastUpdatedUserId
        /// </summary>

        [MaxLength(15)]
        [DataMember(Name = "lastUpdatedUserId")]
        public string? LastUpdatedUserId { get; set; }
    }
}
