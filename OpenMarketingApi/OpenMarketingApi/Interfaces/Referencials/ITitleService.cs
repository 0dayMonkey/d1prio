using OpenMarketingApi.Models.Referencials.Title;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface ITitleService
    {
        Task<List<TitleResponse>> GetAllTitles();
        Task<ActionResult<TitleResponse>> GetTitleById(string id);
        Task<ActionResult<TitleResponse>> CreateTitle(string userId, string? CorrelationId, int? SiteOriginId, TitlePOST addressType);
        Task<ActionResult> EditTitle(string userId, string? CorrelationId, int? SiteOriginId, string id, TitlePUT addressType);
        Task<ActionResult> DeleteTitle(string userId, string? CorrelationId, int? SiteOriginId, string id, ServiceAction body);
    }
}
