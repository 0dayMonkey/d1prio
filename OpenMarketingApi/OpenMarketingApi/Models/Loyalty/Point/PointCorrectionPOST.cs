namespace OpenMarketingApi.Models.Loyalty.Point
{
    [DataContract]
    public class PointCorrectionPOST : PointTransactionBasePOST
    {
        [Required]
        [DataMember(Name = "points")]
        public decimal Points { get; set; }

        [Required]
        [DataMember(Name = "impactLifePoints")]
        public bool ImpactLifePoints { get; set; }

        [Required]
        [DataMember(Name = "details")]
        public PointCorrectionDetailPOST[] Details { get; set; }
    }
}
