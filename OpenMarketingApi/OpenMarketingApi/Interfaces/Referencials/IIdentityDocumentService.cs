using OpenMarketingApi.Models.Referencials.IdentityDocument;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface IIdentityDocumentService
    {
        Task<List<IdDocumentType>> GetAllIdentityDocumentType();
        Task<ActionResult<IdDocumentType>> GetIdentityDocumentTypeAsync(string id);
        Task<ActionResult<IdDocumentType>> CreateIdentityDocumentTypeAsync(string userId, string? CorrelationId, int? SiteOriginId, IdDocumentTypePOST idDocType);
        Task<ActionResult> EditIdentityDocumentTypeAsync(string userId, string? CorrelationId, int? SiteOriginId, string id, IdDocumentTypePUT idDocType);
        Task<ActionResult> DeleteIdentityDocumentType(string userId, string? CorrelationId, int? SiteOriginId, string id, ServiceAction body);
    }
}
