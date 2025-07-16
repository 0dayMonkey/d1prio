using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface ICountryService
    {
        Task<SearchResult<Country>> GetAllCountries(string searchModel);
        Task<ActionResult<Country>> GetCountryById(string id);
        Task<ActionResult<Country>> CreateCountry(string userId, string? CorrelationId, int? SiteOriginId, CountryPOST country);
        Task<ActionResult> EditCountry(string userId, string? CorrelationId, int? SiteOriginId, string id, CountryPUT country);
        Task<ActionResult> DeleteCountry(string userId, string? CorrelationId, int? SiteOriginId, string id, ServiceAction body);
    }
}
