namespace AddressLocation.Domain.Models
{
    public partial class AddressLevelStructure
    {
        public string CountryId { get; set; }

        public AddressLevelDescription AddressLevel1Description { get; set; }
        public AddressLevelDescription AddressLevel2Description { get; set; }
        public AddressLevelDescription AddressLevel3Description { get; set; }

        public DateTime? LastUpdatedTimestamp { get; set; }

        public string UserId { get; set; }
    }
}
