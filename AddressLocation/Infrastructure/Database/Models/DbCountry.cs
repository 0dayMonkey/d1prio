namespace AddressLocation.Infrastructure.Database.Models
{
    public partial class DbCountry
    {
        public string Id { get; set; } = null!;
        public string? LongLabel { get; set; }
        public string? ShortLabel { get; set; }
        public string? LangId { get; set; }
        public bool IsMailAllowed { get; set; }
        public bool IsPostcodeOnTheRight { get; set; }
        public string? ISO2 { get; set; }
        public int CountryNumber { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
        public string? NationalityLabel { get; set; }
        public string? SocialSecurityMask { get; set; }

        public virtual DbAddressLevelStructure AddressLevelStructure { get; set; } = null!;
        public virtual DbAddressPath? AddressPath { get; set; }
    }
}
