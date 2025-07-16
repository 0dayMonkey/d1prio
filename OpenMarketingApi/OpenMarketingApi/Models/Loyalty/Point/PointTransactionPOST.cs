using OpenMarketingApi.Models.Loyalty.Transaction;

namespace OpenMarketingApi.Models.Loyalty.Point
{
    [DataContract]
    public class PointTransactionPOST : PointTransactionBasePOST
    {
        [Required]
        [DataMember(Name = "quantity")]
        public int Quantity { get; set; }

        [Required]
        [DataMember(Name = "type")]
        public TransactionType Type { get; set; }

        [Required]
        [DataMember(Name = "tenderId")]
        public string TenderId { get; set; }

        [Required]
        [DataMember(Name = "pointAmount")]
        public double PointAmount { get; set; }
    }
}
