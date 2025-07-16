namespace OpenMarketingApi.Models.Loyalty.Credential
{
    public class LoyaltyCredentialDELETE
    {
        public string PlayerId { get; set; }
        public string CredentialId { get; set; }
        public string UserId { get; set; }
        public DateTime? LastUpdatedTimestamp{ get; set; }
    }
}
