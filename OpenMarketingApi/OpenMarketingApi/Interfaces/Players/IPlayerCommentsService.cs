using OpenMarketingApi.Models.Player.Comment;

namespace OpenMarketingApi.Interfaces.Players
{
    public interface IPlayerCommentsService
    {
        Task<List<PlayerCommentResponse>> GetAllComment(string playerId);
        Task<ActionResult<PlayerCommentResponse>> GetCommentById(string id, string playerId);
        Task<ActionResult<PlayerCommentResponse>> CreateComment(string userId, string? CorrelationId, int? SiteOriginId, PlayerCommentPOST address, string playerId);
        Task<ActionResult> EditComment(string userId, string? CorrelationId, int? SiteOriginId, string id, PlayerCommentPUT address, string playerId);
        Task<ActionResult> DeleteComment(string userId, string? CorrelationId, int? SiteOriginId, string id, string playerId, ServiceAction body);
    }
}
