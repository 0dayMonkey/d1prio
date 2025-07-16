namespace OpenMarketingApi.Interfaces.Referencials
{
    public interface INationalityService
    {
        Task<List<Nationality>> GetNationalities();
    }
}
