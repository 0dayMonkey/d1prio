namespace AddressLocation.Domain.Models
{
    public partial class City 
    {
        public string Id { get; set; } = null!;

        public string? LongLabel { get; set; }

        public string? ShortLabel { get; set; }

        public DateTime? LastUpdatedTimestamp { get; set; }
        
        public ICollection<PostalCode>? PostalCodes { get; set; }

        public string CountryId { get; set; } = null!;
		
		public string? UserId { get; set; }

        public AddressLevel1? AddressLevel1 { get; set; }
        public AddressLevel2? AddressLevel2 { get; set; }
        public AddressLevel3? AddressLevel3 { get; set; }

    }
}
