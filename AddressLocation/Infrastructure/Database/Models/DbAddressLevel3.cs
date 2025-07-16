using AddressLocation.Infrastructure.Database.Models;

namespace AddressLocation.Domain.Models
{
    public partial class DbAddressLevel3
    {
        public string Id { get; set; } = null!;
        public string? Abbreviation { get; set; }
        public string? LongLabel { get; set; }
        public string? ShortLabel { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
        public virtual DbAddressPath? AsParentAddressPath { get; set; }
        public virtual DbAddressPath? AsChildAddressPath { get; set; }
    }
}
