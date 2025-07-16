using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Repositories;
using AddressLocation.Domain.Services.Interfaces;
using Framework.Exceptions;

namespace AddressLocation.Domain.Services
{
    public class AddressLevelRepositoryFactory : IAddressLevelRepositoryFactory
    {
        public readonly IServiceProvider _serviceProvider;
        public AddressLevelRepositoryFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public IAddressLevelRepository CreateRepository(int level)
        {
            switch (level)
            {
                case (int)AddressLevelEnum.Level1:
                    return (IAddressLevelRepository)_serviceProvider.GetService(typeof(IAddressLevel1Repository));
                case (int)AddressLevelEnum.Level2:
                    return (IAddressLevelRepository)_serviceProvider.GetService(typeof(IAddressLevel2Repository));
                case (int)AddressLevelEnum.Level3:
                    return (IAddressLevelRepository)_serviceProvider.GetService(typeof(IAddressLevel3Repository));
                default:
                    throw new DomainNotFoundException(DomainNotFoundKey.AddressLevelIsNotFound.ToString(), $"This AddressLevel {level} is not available");
            }
        }
    }
}
