using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Credentials
{
    public class CredentialPATCH : IServiceAction
    {
        public bool? IsManualLocked { get; set; }
        public string? PinCode { get; set; }
        public string? PinCodeHash { get; set; }
        public int? FailedAttempt { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }

        public bool IsAllNull()
        {
            return IsManualLocked == null && PinCode == null && PinCodeHash == null && FailedAttempt == null && LastUpdatedTimestamp == null;
        }
    }
}
