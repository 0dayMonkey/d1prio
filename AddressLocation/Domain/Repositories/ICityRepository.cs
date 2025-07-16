using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Repositories
{
    public interface ICityRepository : IRepository<City>, ISearch<City>, IChangeTracking<City>, IChangeTracking
    {
        bool IsUsed(string id);
    }
}
