using OpenMarketingApi.Models.Player.Document;

namespace OpenMarketingApi.Interfaces.Players
{
    public interface IPlayerDocumentsPicturesService
    {
        Task<ActionResult<PlayerDocumentsPicturesResponse>> CreatePlayerDocumentsPicture(string userId, string? correlationId, int? SiteOriginId, PlayerDocumentsPicturesPOST body, string playerId, string id, int index);
        Task<ActionResult> EditPlayerDocumentsPicture(string userId, string? correlationId, int? siteOrigin, string id, PlayerDocumentsPicturesPUT body, string playerId, string index);
        Task<ActionResult> DeletePlayerDocumentsPicture(string userId, string? correlationId, int? siteOrigin, string id, string playerId, string index);
        Task<ActionResult<PlayerDocumentsPicturesResponse>> GetPlayerDocumentsPicture(string playerId, string id, int index);
    }
}
