namespace OpenMarketingApi.Models.Referencials.Address
{
    public class AddressLevelPUT
    {
        public string ParentId { get; set; } = null!;
        public string? Abbreviation { get; set; }
        public string? LongLabel { get; set; }
        public string? ShortLabel { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
