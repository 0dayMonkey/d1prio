using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Repositories
{
    public interface IAddressLevel1Repository : IRepository<AddressLevel1>, IChangeTracking<AddressLevel1>, IChangeTracking, IAddressLevelRepository
    {
    }
}
