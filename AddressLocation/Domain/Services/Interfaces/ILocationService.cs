namespace AddressLocation.Domain.Services.Interfaces
{
    public interface IAddressLocationService
    {
        Task ValidateAddressLocationsIdUnicity(string? id);
    }
}
