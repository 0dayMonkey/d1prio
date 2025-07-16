namespace OpenMarketingApi.Models.Referencials.Address
{
    public partial class CityReference
    {
        public string Id { get; set; } = null!;

        public string? ShortLabel { get; set; } = null!;

        public string? LongLabel { get; set; } = null!;

        public AddressLevelRef? AddressLevel1 { get; set; }
        public AddressLevelRef? AddressLevel2 { get; set; }
        public AddressLevelRef? AddressLevel3 { get; set; }
    }
}
