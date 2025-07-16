namespace OpenMarketingApi.Models.Player.Incentive
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlayerIncentive
    {
        /// <summary>
        /// Gets or Sets Enabled
        /// </summary>

        [DataMember(Name = "enabled")]
        public bool? Enabled { get; set; }

        /// <summary>
        /// Gets or Sets PointsIncentive
        /// </summary>

        [DataMember(Name = "pointsIncentive")]
        public PlayerLevelIncentive PointsIncentive { get; set; }
    }
}
