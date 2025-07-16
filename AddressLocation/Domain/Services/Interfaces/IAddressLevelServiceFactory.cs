namespace AddressLocation.Domain.Services.Interfaces
{
    public interface IAddressLevelServiceFactory
    {
        IAddressLevelService RetreiveAddressLevelService(int level);
    }
}
