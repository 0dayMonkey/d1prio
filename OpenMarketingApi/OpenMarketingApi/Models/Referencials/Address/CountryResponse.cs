namespace OpenMarketingApi.Models.Referencials.Address
{
    public partial class CountryResponse
    {
        public string Id { get; set; }
        public string? LongLabel { get; set; }
        public string? ShortLabel { get; set; }
        public string LangId { get; set; }
        public bool IsMailAllowed { get; set; }
        public bool IsPostcodeOnTheRight { get; set; }
        public string ISO2 { get; set; }
        public int CountryNumber { get; set; }
        public DateTime LastUpdatedTimestamp { get; set; }
        public string? NationalityLabel { get; set; }
        public string? SocialSecurityMask { get; set; }
    }

    public partial class CountryList
    {
        public List<CountryResponse>? Data { get; set; }
        public ulong Length { get; set; }
    }


}
