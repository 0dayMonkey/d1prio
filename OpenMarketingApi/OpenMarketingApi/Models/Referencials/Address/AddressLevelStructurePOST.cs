namespace OpenMarketingApi.Models.Referencials.Address
{
    public class AddressLevelStructurePOST
    {
        public AddressLevelDescriptionPOST? AddressLevel1Description { get; set; }
        public AddressLevelDescriptionPOST? AddressLevel2Description { get; set; }
        public AddressLevelDescriptionPOST? AddressLevel3Description { get; set; }

        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
