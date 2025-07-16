
namespace AddressLocation.Domain.Models
{
    public partial class DbOutboxMessage
    {
        public long Id { get; set; }

        public DateTime EventTimestampUtc { get; set; } = DateTime.UtcNow;

        public DateTime? ExpirationTimestampUtc { get; set; }

        public string? CorrelationId { get; set; }

        public int? SiteOriginId { get; set; }

        public string? RoutingKey { get; set; }

        public string? Exchange { get; set; }

        public string? Context { get; set; }

        public string? ConsistentHash { get; set; }

        public bool IsReliable { get; set; }

        public string Data { get; set; } = string.Empty;
        
    }
}
