namespace OpenMarketingApi.Models.Referencials.Address
{
    public class AddressLevelStructurePUT
    {
        public AddressLevelDescriptionPUT? AddressLevel1Description { get; set; }
        public AddressLevelDescriptionPUT? AddressLevel2Description { get; set; }
        public AddressLevelDescriptionPUT? AddressLevel3Description { get; set; }

        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
