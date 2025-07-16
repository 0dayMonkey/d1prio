using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface IAddressLevelStructureService
    {
        Task<AddressLevelStructureResponse> GetAddressLevelStructureFromCountry(string countryId);
        Task<ActionResult<AddressLevelStructureResponse>> CreateAddressLevelStructureForCountry(string userId, string? CorrelationId, int? SiteOriginId, AddressLevelStructurePOST item, string countryId);
        Task<ActionResult> UpdateAddressLevelStructureForCountry(string userId, string? CorrelationId, int? SiteOriginId, AddressLevelStructurePUT item, string countryId);
    }
}
