using AddressLocation.Domain.Models;

namespace AddressLocation.Domain.Repositories
{
    public interface IAddressLevelRepository
    {
        Task<List<AddressLevel>> ListAllFromCountry(string countryId, bool tracking = false);
        Task<bool> IsLinkedToChildren(string id);
        Task<AddressLevel?> SingleOrDefaultFromCountry(string countryId, string id, bool tracking = false);
    }
}
