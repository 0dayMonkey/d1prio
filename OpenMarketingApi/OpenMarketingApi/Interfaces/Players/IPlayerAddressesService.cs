using OpenMarketingApi.Models.Player.Address;

namespace OpenMarketingApi.Interfaces.Players
{
    public interface IPlayerAddressesService
    {
        Task<List<PlayerAddressResponse>> GetAllAddress(string playerId);
        Task<ActionResult<PlayerAddressResponse>> GetAddressById(string id, string playerId);
        Task<ActionResult<PlayerAddressResponse>> CreateAddress(string userId, string? CorrelationId, int? SiteOriginId, PlayerAddressPOST address, string playerId);
        Task<ActionResult> EditAddress(string userId, string? CorrelationId, int? SiteOriginId, string id, PlayerAddressPUT address, string playerId);
        Task<ActionResult> DeleteAddress(string userId, string? CorrelationId, int? SiteOriginId, string id, string playerId, ServiceAction body);
    }
}