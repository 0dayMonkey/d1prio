using OpenMarketingApi.Models.Referencials.Occupation;
using OpenMarketingApi.Models.Referencials.SocioProfessional;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface ISocioProfessionalCategoryAndOccupationService
    {
        Task<List<SocioProfessionalCategory>> GetSocioProfessionalCategories();
        Task<SocioProfessionalCategory> GetSocioProfessionalCategoryById(string id);
        Task<List<Occupation>> GetOccupationsBySocioProfessionalCategory(string SocioProfessionalCategoryId);
        Task<Occupation> GetOccupationBySocioProfessionalCategory(string SocioProfessionalCategoryId, string id);

        Task<ActionResult<Occupation>> CreateOccupationInSocioProfessionalCategory(string userId, string? CorrelationId, int? SiteOriginId, OccupationPOST body, string socioProfessionalCategoryId);
        Task<ActionResult<SocioProfessionalCategory>> CreateSocioProfessionalCategories(string userId, string? CorrelationId, int? SiteOriginId, SocioProfessionalCategoryPOST body);

        Task<ActionResult> UpdateOccupation(string userId, string? correlationId, int? siteOrigin, OccupationPUT body, string socioProfessionalCategoryId, string occupationId);
        Task<ActionResult> UpdateSocioProfessionalCategory(string userId, string? correlationId, int? siteOrigin, SocioProfessionalCategoryPUT body, string socioProfessionalCategoryId);
        Task<ActionResult> DeleteSocioProfessionalCategory(string userId, string? correlationId, int? siteOrigin, string socioProfessionalCategoryId, ServiceAction body);
        Task<ActionResult> DeleteOccupation(string userId, string? correlationId, int? siteOrigin, string socioProfessionalCategoryId, string occupationId, ServiceAction body);
    }
}
