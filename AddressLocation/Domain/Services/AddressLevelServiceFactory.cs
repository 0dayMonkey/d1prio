using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Services.Interfaces;
using Framework.Exceptions;

namespace AddressLocation.Domain.Services
{
    public class AddressLevelServiceFactory : IAddressLevelServiceFactory
    {
        public readonly IServiceProvider _serviceProvider;
        public AddressLevelServiceFactory(IServiceProvider serviceProvider) {
            _serviceProvider = serviceProvider;
        }
        public IAddressLevelService RetreiveAddressLevelService(int level)
        {
            switch (level)
            {
                case (int)AddressLevelEnum.Level1:
                    return (IAddressLevelService)_serviceProvider.GetService(typeof(IAddressLevel1Service));
                case (int)AddressLevelEnum.Level2:
                    return (IAddressLevelService)_serviceProvider.GetService(typeof(IAddressLevel2Service));
                case (int)AddressLevelEnum.Level3:
                    return (IAddressLevelService)_serviceProvider.GetService(typeof(IAddressLevel3Service));
                default:
                    throw new DomainViolationException(DomainViolationKey.AddressLevelIsUnknown.ToString(), $"This AddressLevel {level} is not available");
            }
        }
    }
}
