namespace OpenMarketingApi.Models.Player.Incentive
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlayerLevelIncentive
    {
        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        //public enum StatusEnum
        //{
        //    /// <summary>
        //    /// Enum ToBeEligibleToUpgradeEnum for ToBeEligibleToUpgrade
        //    /// </summary>
        //    [EnumMember(Value = "ToBeEligibleToUpgrade")]
        //    ToBeEligibleToUpgradeEnum = 0,
        //    /// <summary>
        //    /// Enum ToNotBeDowngradedEnum for ToNotBeDowngraded
        //    /// </summary>
        //    [EnumMember(Value = "ToNotBeDowngraded")]
        //    ToNotBeDowngradedEnum = 1,
        //    /// <summary>
        //    /// Enum EligibleForUpgradeEnum for EligibleForUpgrade
        //    /// </summary>
        //    [EnumMember(Value = "EligibleForUpgrade")]
        //    EligibleForUpgradeEnum = 2
        //}

        /// <summary>
        /// Gets or Sets Status
        /// </summary>
        [Required]

        [DataMember(Name = "status")]
        public string Status { get; set; }

        /// <summary>
        /// Missing points to be upgraded or not being downgraded
        /// </summary>
        /// <value>Missing points to be upgraded or not being downgraded</value>

        [DataMember(Name = "neededPoints")]
        public double? NeededPoints { get; set; }

        /// <summary>
        /// Date when points need to be won
        /// </summary>
        /// <value>Date when points need to be won</value>

        [DataMember(Name = "qualificationDate")]
        public DateTime? QualificationDate { get; set; }

        /// <summary>
        /// Status: ToBeEligible : Total points for upgrade regarding its level  Status: ToNotBeDowngraded : Total point needed to stay on the level 
        /// </summary>
        /// <value>Status: ToBeEligible : Total points for upgrade regarding its level  Status: ToNotBeDowngraded : Total point needed to stay on the level </value>

        [DataMember(Name = "ruleThreshold")]
        public double? RuleThreshold { get; set; }
    }
}
