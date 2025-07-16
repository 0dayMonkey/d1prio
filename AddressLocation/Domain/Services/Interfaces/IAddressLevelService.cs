using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Services.Interfaces
{
    public interface IAddressLevelService
    {
        public Task AddLevel(AddressLevel source, ChangeContext? changeContext);
        public Task UpdateLevel(AddressLevel source, ChangeContext? changeContext);
        public Task DeleteLevel(AddressLevel source, ChangeContext? changeContext);
        public Task<AddressLevel> CheckAddressLevelExistForDelete(string id, string countryId);
        public Task SaveChangesAsync();
    }
}
