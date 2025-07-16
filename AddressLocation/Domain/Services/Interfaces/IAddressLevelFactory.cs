using AddressLocation.Domain.Models;

namespace AddressLocation.Domain.Services.Interfaces
{
    public interface IAddressLevelFactory
    {
        AddressLevel RetreiveAddressLevel(int level);
    }
}
