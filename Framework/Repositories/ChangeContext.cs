namespace Framework.Repositories
{
    public class ChangeContext
    {
        public string? CorrelationId { get; init; }

        public int? SiteOriginId { get; init; }

        public ChangeContext(string? correlationId, int? siteOriginId)
        {
            CorrelationId = correlationId;
            SiteOriginId = siteOriginId;
        }
    }
}
