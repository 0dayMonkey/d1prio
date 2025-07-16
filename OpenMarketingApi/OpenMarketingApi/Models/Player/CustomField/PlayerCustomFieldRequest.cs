namespace OpenMarketingApi.Models.Player.CustomField
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlayerCustomFieldRequest
    {
        /// <summary>
        /// identifier PlayerField where isCustomField is true
        /// </summary>
        /// <value>identifier PlayerField where isCustomField is true</value>

        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
