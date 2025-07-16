using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Services.Interfaces
{
    public interface ICityService
    {
        public Task Add(City source, ChangeContext? changeContext);
        public Task Update(City source, ChangeContext? changeContext);
        Task EnrichAddressLevels(City city);
        Task EnrichAddressLevelsForPUT(City city);
    }
}
