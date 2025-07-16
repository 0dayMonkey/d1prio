namespace AddressLocation.Infrastructure.Database.Models
{
    public partial class DbAddressLevelStructure
    {
        public string CountryId { get; set; } = null!;
        public decimal AddressLevel1AbbreviationId { get; set; }
        public decimal AddressLevel1DescriptionId { get; set; }
        public decimal AddressLevel2AbbreviationId { get; set; }
        public decimal AddressLevel2DescriptionId { get; set; }
        public decimal AddressLevel3AbbreviationId { get; set; }
        public decimal AddressLevel3DescriptionId { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }

        public virtual DbCountry Country { get; set; } = null!;

        public virtual DbAddressLevelLabel? AddressLevel1Abbreviation { get; set; }
        public virtual DbAddressLevelLabel? AddressLevel2Abbreviation { get; set; }
        public virtual DbAddressLevelLabel? AddressLevel3Abbreviation { get; set; }

        public virtual DbAddressLevelLabel? AddressLevel1Description { get; set; }
        public virtual DbAddressLevelLabel? AddressLevel2Description { get; set; }
        public virtual DbAddressLevelLabel? AddressLevel3Description { get; set; }
    }
}
