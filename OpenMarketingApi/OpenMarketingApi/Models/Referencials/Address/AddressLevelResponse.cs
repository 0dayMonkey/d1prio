namespace OpenMarketingApi.Models.Referencials.Address
{
    public class AddressLevelResponse
    {
        public string Id { get; set; } = null!;
        public string? Abbreviation { get; set; }
        public string? ParentId { get; set; }
        public string? LongLabel { get; set; }
        public string? ShortLabel { get; set; }
        public DateTime LastUpdatedTimestamp { get; set; }
    }
}
