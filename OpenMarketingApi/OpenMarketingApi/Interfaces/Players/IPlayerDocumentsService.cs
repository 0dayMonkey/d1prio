using OpenMarketingApi.Models.Player.Document;

namespace OpenMarketingApi.Interfaces.Players
{
    public interface IPlayerDocumentsService
    {
        Task<List<PlayerDocumentResponse>> GetAllDocument(string playerId);
        Task<ActionResult<PlayerDocumentResponse>> GetDocumentById(string id, string playerId);
        Task<ActionResult<PlayerDocumentResponse>> CreateDocument(string userId, string? CorrelationId, int? SiteOriginId, PlayerDocumentPOST address, string playerId);
        Task<ActionResult> EditDocument(string userId, string? CorrelationId, int? SiteOriginId, string id, PlayerDocumentPUT address, string playerId);
        Task<ActionResult> DeleteDocument(string userId, string? CorrelationId, int? SiteOriginId, string id, string playerId, ServiceAction body);
    }
}
