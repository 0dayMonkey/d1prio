namespace OpenMarketingApi.Models.Loyalty.Transaction
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class TransactionTender
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
    }
}
