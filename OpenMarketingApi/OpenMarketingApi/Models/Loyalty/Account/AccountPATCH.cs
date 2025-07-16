using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Loyalty.Account
{
    [DataContract]
    public class AccountPATCH : IServiceAction
    {
        [DataMember(Name = "tierId")]
        public string TierId { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
