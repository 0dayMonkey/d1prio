using OpenMarketingApi.Models.Player.CustomField;

namespace OpenMarketingApi.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class PlayerField
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name="id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or Sets Label
        /// </summary>

        [DataMember(Name="label")]
        public string Label { get; set; }

        /// <summary>
        /// Gets or Sets Type
        /// </summary>

        [DataMember(Name = "type")]
        public CustomFieldType Type { get; set; }
    }
}
