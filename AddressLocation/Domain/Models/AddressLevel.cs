using AddressLocation.Domain.Enums;

namespace AddressLocation.Domain.Models
{
    public abstract class AddressLevel
    {
        public string? Id { get; set; }
        public string CountryId { get; set; }
        public string? ParentId { get; set; }
        public AddressLevelEnum? ParentLevel { get; set; }
        public abstract AddressLevelEnum Level { get; }
        public string? Abbreviation { get; set; }
        public string? LongLabel { get; set; }
        public string? ShortLabel { get; set; }
        public string? UserId { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
