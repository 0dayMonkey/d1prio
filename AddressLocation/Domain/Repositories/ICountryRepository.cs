using AddressLocation.Domain.Models;
using Framework.Repositories;

namespace AddressLocation.Domain.Repositories
{
    public interface ICountryRepository : IRepository<Country>, ISearch<Country>, IChangeTracking<Country>, IChangeTracking
    {
        Task<List<Country>> GetCountriesforNationalities();

        Task<bool> LanguageExistsAsync(string languageId);

        Task<bool> HasAddressLevelOrCityLinked(string countryId);

        bool IsUsed(string id);
    }
}
