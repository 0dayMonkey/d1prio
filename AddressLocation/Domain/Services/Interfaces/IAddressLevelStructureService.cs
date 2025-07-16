using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Services.Interfaces
{
    public interface IAddressLevelStructureService
    {
        public Task Add(AddressLevelStructure source, ChangeContext changeContext);
        public Task Update(AddressLevelStructure source, ChangeContext changeContext);
    }
}
