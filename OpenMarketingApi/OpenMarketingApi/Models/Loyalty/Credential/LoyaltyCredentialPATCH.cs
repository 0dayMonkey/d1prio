namespace OpenMarketingApi.Models.Loyalty.Credential
{
    public class LoyaltyCredentialPATCH
    {
        public string PlayerId { get; set; }
        public bool? IsManualLocked { get; set; }
        public string? PinCode { get; set; }
        public string? PinCodeHash { get; set; }
        public string UserId { get; set; }
        public int? FailedAttempt { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
        public string CredentialId { get; set; }
    }
}
