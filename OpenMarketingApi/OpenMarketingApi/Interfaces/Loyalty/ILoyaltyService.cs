using OpenMarketingApi.Models.Loyalty.Account;
using OpenMarketingApi.Models.Loyalty.Configuration;
using OpenMarketingApi.Models.Loyalty.Credential;
using OpenMarketingApi.Models.Loyalty.Point;
using OpenMarketingApi.Models.Loyalty.Tier;

namespace OpenMarketingApi.Interfaces.Loyalty
{
    public interface ILoyaltyService
    {
        Task<ActionResult<AccountResponse>> GetLoyaltyAccount(string playerId);
        Task<ActionResult> UpdateLoyaltyAccount(string? CorrelationId, int? SiteOriginId, string UserId, string? ClientId, string PlayerId, AccountPATCH body);
        Task<ActionResult<AccountResponse>> CreateLoyaltyAccount(string? correlationId, int? siteOriginId, string UserId, string ClientId, AccountPOST body);
        Task<ActionResult> DeleteLoyaltyAccount(string? correlationId, int? siteOriginId, string UserId, string PlayerId, ServiceAction body);
        Task<SearchResult<PointTransaction>> GetPointTransactions(string playerId, string searchModel);
        Task<ActionResult<PointConversion>> GetPointsConversion(string playerId);
        Task<ActionResult<List<TierResponse>>> GetLoyaltyTiers();
        Task<ActionResult<TierResponse>> GetLoyaltyTier(string tierId);
        Task<ActionResult<TierResponse>> CreateLoyaltyTier(string userId, string? CorrelationId, int? SiteOriginId, TierPOST body);
        Task<ActionResult> UpdateLoyaltyTier(string userId, string? CorrelationId, int? SiteOriginId, string tierId, TierPUT body);
        Task<ActionResult<ConfigurationResponse>> GetLoyaltyTierConfiguration();
        Task<ActionResult> UpdateLoyaltyTierConfiguration(string userId, string? correlationId, int? siteOriginId, ConfigurationPUT body);
        Task<PointTransaction> CreatePointTransaction(string userId, string? correlationId, int? siteOriginId, string playerId, PointTransactionBasePOST body);
        Task<ActionResult<List<Credential>>> GetAccountCredentials(string playerId);
        Task<ActionResult<List<Credential>>> GetCredentialById(string id, string playerId);
        Task<ActionResult<List<Credential>>> CreateCredentials(string? CorrelationId, int? SiteOriginId, LoyaltyCredentialPOST playerCredentialPost);
        Task<ActionResult> PatchCredential(string? CorrelationId, int? SiteOriginId, LoyaltyCredentialPATCH playerCredentialPatch);
        Task<ActionResult> DeleteCredential(string? CorrelationId, int? SiteOriginId, LoyaltyCredentialDELETE delete);
    }
}
