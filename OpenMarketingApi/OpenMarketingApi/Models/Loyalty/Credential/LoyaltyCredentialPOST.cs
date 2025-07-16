namespace OpenMarketingApi.Models.Loyalty.Credential
{

    public class LoyaltyCredentialPOST
    {
        public string PlayerId { get; set; }
        public LoyaltyCredentialData[] PlayerCredentials { get; set; }
        public int? IssuingSiteId { get; set; }
        public DateTime? CreationTimestamp { get; set; }
        public DateTime? BeginValidityTimestamp { get; set; }
        public DateTime? EndValidityTimestamp { get; set; }
        public List<SupportType> SupportType { get; set; }
        public string? SupportId { get; set; }
        public string UserId { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
