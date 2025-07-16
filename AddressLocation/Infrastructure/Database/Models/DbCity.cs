namespace AddressLocation.Infrastructure.Database.Models
{
    public partial class DbCity 
    {
        
        public string Id { get; set; } = null!;

        public string LongLabel { get; set; }

        public string ShortLabel { get; set; }

        public DateTime? LastUpdatedTimestamp { get; set; }

        public virtual ICollection<DbPostalCode>? PostalCodes { get; set; }

        public virtual DbAddressPath? AddressPath { get; set; }   
    }
}
