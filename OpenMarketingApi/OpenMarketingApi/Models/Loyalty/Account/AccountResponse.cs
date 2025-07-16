using OpenMarketingApi.Models.Loyalty.Tier;
using OpenMarketingApi.Models.Player.Incentive;

namespace OpenMarketingApi.Models.Loyalty.Account
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class AccountResponse
    {
        /// <summary>
        /// Gets or Sets EnrollmentDate
        /// </summary>

        [DataMember(Name = "enrollmentDate")]
        public DateTime? EnrollmentDate { get; set; }

        /// <summary>
        /// Gets or Sets PointBalance
        /// </summary>

        [DataMember(Name = "pointBalance")]
        public decimal? PointBalance { get; set; }

        /// <summary>
        /// Gets or Sets Tier
        /// </summary>

        [DataMember(Name = "tier")]
        public TierResponse Tier { get; set; }

        [DataMember(Name = "levelIncentive")]
        public PlayerLevelIncentive LevelIncentive { get; set; }
    }
}
