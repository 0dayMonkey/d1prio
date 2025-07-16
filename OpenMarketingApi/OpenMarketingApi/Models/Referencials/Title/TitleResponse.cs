namespace OpenMarketingApi.Models.Referencials.Title
{
    public class TitleResponse
    {
        public string Id { get; set; } = null!;

        public string LongLabel { get; set; }

        public string ShortLabel { get; set; }

        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
