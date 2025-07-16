namespace AddressLocation.Domain.Models
{
    public partial class PostalCode
    {
        public string Code { get; set; } = null!;

        public DateTime? LastUpdatedTimestamp { get; set; }
		
		public string? UserId {get; set; }
    }
}
