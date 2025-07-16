namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class Order
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id", EmitDefaultValue = false)]
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets PlayerId
        /// </summary>
        [DataMember(Name = "playerId", EmitDefaultValue = false)]
        [JsonPropertyName("playerId")]
        public string PlayerId { get; set; }

        /// <summary>
        /// Gets or Sets Items
        /// </summary>
        [DataMember(Name = "items", EmitDefaultValue = false)]
        [JsonPropertyName("items")]
        public List<OrderItem> Items { get; set; }

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [DataMember(Name = "status", EmitDefaultValue = false)]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or Sets Total
        /// </summary>
        [DataMember(Name = "total", EmitDefaultValue = false)]
        [JsonPropertyName("total")]
        public decimal? Total { get; set; }

        /// <summary>
        /// Gets or Sets CreationTimestamp
        /// </summary>
        [DataMember(Name = "creationTimestamp", EmitDefaultValue = false)]
        [JsonPropertyName("creationTimestamp")]
        public DateTime? CreationTimestamp { get; set; }
    }
}
