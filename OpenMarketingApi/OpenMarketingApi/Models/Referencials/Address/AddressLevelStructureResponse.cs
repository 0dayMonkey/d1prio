namespace OpenMarketingApi.Models.Referencials.Address
{
    public class AddressLevelStructureResponse
    {
        public string CountryId { get; set; }
        public AddressLevelDescription AddressLevel1 { get; set; }
        public AddressLevelDescription AddressLevel2 { get; set; }
        public AddressLevelDescription AddressLevel3 { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
