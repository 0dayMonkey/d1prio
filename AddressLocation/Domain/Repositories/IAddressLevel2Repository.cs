using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Repositories
{
    public interface IAddressLevel2Repository : IRepository<AddressLevel2>, IChangeTracking<AddressLevel2>, IChangeTracking, IAddressLevelRepository
    {
    }
}
