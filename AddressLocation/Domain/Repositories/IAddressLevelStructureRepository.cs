using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Repositories
{
    public interface IAddressLevelStructureRepository : IRepository<AddressLevelStructure>, IChangeTracking<AddressLevelStructure>, IChangeTracking
    {
    }
}
