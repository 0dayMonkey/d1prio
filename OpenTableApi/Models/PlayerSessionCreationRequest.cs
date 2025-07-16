using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OpenTableApi.Models
{
    public class PlayerSessionCreationRequest : PlayerSessionCommonInfo
    {
        [Required]
        [DataMember(Name = "tableCode")]
        public string TableCode { get; set; }

        [Required]
        [DataMember(Name = "siteID")]
        public int SiteID { get; set; }

        [Required]
        [DataMember(Name = "playerID")]
        public string PlayerID { get; set; }
    }
}
