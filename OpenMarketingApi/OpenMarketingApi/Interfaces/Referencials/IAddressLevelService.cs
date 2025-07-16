using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface IAddressLevelService
    {
        Task<List<AddressLevelResponse>> GetAddressLevelFromCountry(string countryId, int level);
        Task<ActionResult<AddressLevelResponse>> CreateAddressLevelForCountry(string userId, string? CorrelationId, int? SiteOriginId, AddressLevelPOST item, string countryId, int level);

        Task<ActionResult<AddressLevelResponse>> UpdateAddressLevelForCountry(string userId, string? CorrelationId, int? SiteOriginId, AddressLevelPUT item, string countryId, int level, string id);

        Task<ActionResult<AddressLevelResponse>> DeleteAddressLevelForCountry(string userId, string? CorrelationId, int? SiteOriginId, ServiceAction action, string countryId, int level, string id);
    }
}
