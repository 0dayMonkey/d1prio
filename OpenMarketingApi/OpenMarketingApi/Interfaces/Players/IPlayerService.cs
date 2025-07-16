using OpenMarketingApi.Models.Player.Player;

namespace OpenMarketingApi.Interfaces.Players
{
    public interface IPlayerService
    {
        Task<ActionResult<Player>> GetPlayer(string playerId);
        Task<ActionResult<Player>> CreatePlayer(string userId, string? CorrelationId, int SiteOriginId, PlayerPOST player);
        Task<ActionResult> UpdatePlayer(string userId, string? CorrelationId, int? SiteOriginId, string playerId, PlayerPUT player);

    }
}
