using OpenMarketingApi.Models.Referencials.MaritalStatus;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface IMaritalStatusService
    {
        Task<List<MaritalStatus>> GetAllMaritalStatuses();
        Task<ActionResult<MaritalStatus>> GetMaritalStatus(string id);
        Task<ActionResult<MaritalStatus>> CreateMaritalStatus(string userId, string? CorrelationId, int? SiteOriginId, MaritalStatusPOST maritalStatus);
        Task<ActionResult> EditMaritalStatus(string userId, string? CorrelationId, int? SiteOriginId, string id, MaritalStatusPUT maritalStatus);
        Task<ActionResult> DeleteMaritalStatus(string userId, string? CorrelationId, int? SiteOriginId, string id, ServiceAction body);
    }
}
