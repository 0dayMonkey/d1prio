using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface ICityService
    {
        Task<ActionResult<SearchResult<CityResponse>>> GetCities(string searchModel, string countryId);
        Task<ActionResult<CityResponse>> GetCity(string countryId, string cityId);
        Task<ActionResult<CityResponse>> CreateCity(string userId, string? CorrelationId, int? SiteOriginId, string countryId, CityPOST city);
        Task<ActionResult> UpdateCity(string userId, string? CorrelationId, int? SiteOriginId, string countryId, string cityId, CityPUT city);
        Task<ActionResult> DeleteCity(string userId, string? CorrelationId, int? SiteOriginId, string countryId, string cityId, ServiceAction body);
    }
}
