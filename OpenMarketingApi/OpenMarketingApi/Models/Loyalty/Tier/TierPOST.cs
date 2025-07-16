using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Loyalty.Tier
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class TierPOST : IServiceAction
    {
        /// <summary>
        /// Gets or Sets Id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets ShortLabel
        /// </summary>
        [Required]
        [DataMember(Name = "shortLabel")]
        public string ShortLabel { get; set; }

        /// <summary>
        /// Gets or Sets LongLabel
        /// </summary>
        [Required]
        [DataMember(Name = "longLabel")]
        public string LongLabel { get; set; }

        /// <summary>
        /// Gets or Sets ColorCode
        /// </summary>
        [Required]
        [DataMember(Name = "colorCode")]
        public string ColorCode { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }

        /// <summary>
        /// player tier points booster
        /// </summary>
        /// <value>player tier points booster</value>

        [DataMember(Name = "boostPercentage")]
        public double? BoostPercentage { get; set; }

        [DataMember(Name = "maxDailyPointAtSlot")]
        public long? MaxDailyPointAtSlot { get; set; }

        [DataMember(Name = "welcomePoints")]
        public int? WelcomePoints { get; set; }

        [DataMember(Name = "monthsBeforeDowngrade")]
        public int? MonthsBeforeDowngrade { get; set; }

        [DataMember(Name = "credentialSettings")]
        public CredentialSettings CredentialSettings { get; set; }
    }
}
