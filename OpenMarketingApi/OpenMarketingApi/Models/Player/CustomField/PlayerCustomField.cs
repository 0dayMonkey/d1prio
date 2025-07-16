namespace OpenMarketingApi.Models.Player.CustomField
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlayerCustomField
    {
        /// <summary>
        /// Gets or Sets Field
        /// </summary>

        [DataMember(Name = "field")]
        public PlayerField Field { get; set; }

        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
