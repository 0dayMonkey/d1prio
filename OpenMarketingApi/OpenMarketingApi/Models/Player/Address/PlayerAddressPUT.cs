using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Player.Address
{
    public partial class PlayerAddressPUT : IServiceAction
    {
        [DataMember(Name = "addressTypeId")]
        public int AddressTypeId { get; set; }
        [Required]
        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }
        [Required]
        [DataMember(Name = "cityId")]
        public string CityId { get; set; }
        [Required]
        [DataMember(Name = "countryId")]
        public string CountryId { get; set; }
        [Required]
        [DataMember(Name = "address1")]
        public string Address1 { get; set; } = null!;
        [DataMember(Name = "address2")]
        public string? Address2 { get; set; }
        [DataMember(Name = "address3")]
        public string? Address3 { get; set; }


        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
