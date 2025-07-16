using AddressLocation.Domain.Repositories;

namespace AddressLocation.Domain.Services.Interfaces
{
    public interface IAddressLevelRepositoryFactory
    {
        IAddressLevelRepository CreateRepository(int level);
    }
}
