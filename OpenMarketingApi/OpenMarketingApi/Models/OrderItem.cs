namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class OrderItem
    {
        /// <summary>
        /// Gets or Sets ItemId
        /// </summary>
        [DataMember(Name = "itemId", EmitDefaultValue = false)]
        [JsonPropertyName("itemId")]
        public string ItemId { get; set; }

        /// <summary>
        /// Gets or Sets Quantity
        /// </summary>
        [DataMember(Name = "quantity", EmitDefaultValue = false)]
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// Gets or Sets Total
        /// </summary>
        [DataMember(Name = "total", EmitDefaultValue = false)]
        [JsonPropertyName("total")]
        public decimal? Total { get; set; }
    }
}
