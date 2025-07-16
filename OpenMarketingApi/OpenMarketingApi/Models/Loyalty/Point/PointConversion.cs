namespace OpenMarketingApi.Models.Loyalty.Point
{
    [DataContract]
    public partial class PointConversion
    {
        /// <summary>
        /// Gets or Sets ConvertiblePoints
        /// </summary>
        [Required]

        [DataMember(Name = "convertiblePoints")]
        public double? ConvertiblePoints { get; set; }

        /// <summary>
        /// Gets or Sets PointValue
        /// </summary>
        [Required]

        [DataMember(Name = "pointValue")]
        public double? PointValue { get; set; }
    }

}
