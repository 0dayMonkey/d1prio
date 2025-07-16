using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Referencials.Address
{
    public partial class AddressTypePUT : IServiceAction
    {
        public string LongLabel { get; set; } = null!;
        public string ShortLabel { get; set; } = null!;
        public bool? IsDefault { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }


    }
}
