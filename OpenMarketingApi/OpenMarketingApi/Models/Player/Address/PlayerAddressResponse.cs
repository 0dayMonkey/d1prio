using OpenMarketingApi.Models.Referencials.Address;

namespace OpenMarketingApi.Models.Player.Address
{
    public partial class PlayerAddressResponse
    {
        [DataMember(Name = "addressId")]
        public string? AddressId { get; set; } = null!;
        [DataMember(Name = "addressType")]
        public AddressTypeReference AddressType { get; set; }
        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }
        [DataMember(Name = "city")]
        public CityReference City { get; set; }
        [DataMember(Name = "country")]
        public CountryReference Country { get; set; }
        [DataMember(Name = "address1")]
        public string Address1 { get; set; } = null!;
        [DataMember(Name = "address2")]
        public string? Address2 { get; set; }
        [DataMember(Name = "address3")]
        public string? Address3 { get; set; }


        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime LastUpdatedTimestamp { get; set; }
    }
}
