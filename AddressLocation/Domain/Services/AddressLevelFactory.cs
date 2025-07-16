using AddressLocation.Domain.Enums;
using AddressLocation.Domain.Models;
using AddressLocation.Domain.Services.Interfaces;
using Framework.Exceptions;

namespace AddressLocation.Domain.Services
{
    public class AddressLevelFactory : IAddressLevelFactory
    {
        public AddressLevelFactory() { }

        public AddressLevel RetreiveAddressLevel(int level)
        {
            switch (level) {
                case (int)AddressLevelEnum.Level1:
                    return new AddressLevel1();
                case (int)AddressLevelEnum.Level2:
                    return new AddressLevel2();
                case (int)AddressLevelEnum.Level3:
                    return new AddressLevel3();
                default:
                    throw new DomainViolationException(DomainViolationKey.AddressLevelIsUnknown.ToString(), $"This AddressLevel {level} is not available");
            }
        }
    }
}
