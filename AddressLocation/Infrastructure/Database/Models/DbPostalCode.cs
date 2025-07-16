namespace AddressLocation.Infrastructure.Database.Models
{
    public partial class DbPostalCode
    {
        public DbPostalCode() { }

        public string Code { get; set; } = null!;

        public string CityId { get; set; } = null!;

        public DateTime? LastUpdatedTimestamp { get; set; }

        public virtual DbCity? City { get; internal set; }
    }
}
