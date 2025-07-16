using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface IAddressTypeService
    {
        Task<List<AddressType>> GetAllAddressTypes();
        Task<ActionResult<AddressType>> GetAddressTypeById(int id);
        Task<ActionResult<AddressType>> CreateAddressType(string userId, string? CorrelationId, int? SiteOriginId, AddressTypePOST addressType);
        Task<ActionResult> EditAddressType(string userId, string? CorrelationId, int? SiteOriginId, string id, AddressTypePUT addressType);
        Task<ActionResult> DeleteAddressType(string userId, string? CorrelationId, int? SiteOriginId, string id, ServiceAction body);
    }
}
