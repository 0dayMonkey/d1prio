using OpenMarketingApi.Models.Player.Contact;

namespace OpenMarketingApi.Interfaces.Players
{
    public interface IPlayerContactsService
    {
        Task<PlayerContactsResponse> GetContactDetails(string playerId);
        Task<ActionResult> EditContactDetail(string userId, string? CorrelationId, int? SiteOriginId, string playerId, PlayerContactsPUT contactDetail);
    }
}
