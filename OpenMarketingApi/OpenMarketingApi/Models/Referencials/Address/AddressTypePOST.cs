using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Referencials.Address
{
    public partial class AddressTypePOST : IServiceAction
    {
        public int? Id { get; set; }
        public string LongLabel { get; set; } = null!;
        public string ShortLabel { get; set; } = null!;
        public bool? IsDefault { get; set; } = false;
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
