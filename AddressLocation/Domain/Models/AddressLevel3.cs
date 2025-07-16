using AddressLocation.Domain.Enums;

namespace AddressLocation.Domain.Models
{
    public class AddressLevel3 : AddressLevel
    {
        public override AddressLevelEnum Level { get { return AddressLevelEnum.Level3; } }
    }
}
