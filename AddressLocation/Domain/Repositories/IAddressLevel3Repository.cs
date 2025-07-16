using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Repositories
{
    public interface IAddressLevel3Repository : IRepository<AddressLevel3>, IChangeTracking<AddressLevel3>, IChangeTracking, IAddressLevelRepository

    {
    }
}
