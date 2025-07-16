using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Loyalty.Account
{
    [DataContract]
    public partial class AccountPOST : IServiceAction
    {
        [DataMember(Name = "playerId")]
        public string PlayerId { get; set; }

        [DataMember(Name = "tierId")]
        public string TierId { get; set; }

        [DataMember(Name = "sponsorId")]
        public string? SponsorId { get; set; }

        [DataMember(Name = "enrollmentDate")]
        public DateTime? EnrollmentDate { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
