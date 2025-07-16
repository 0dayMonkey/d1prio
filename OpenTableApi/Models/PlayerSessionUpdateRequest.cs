using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OpenTableApi.Models
{
    public class PlayerSessionUpdateRequest : PlayerSessionCommonInfo
    {
        [Required]
        [DataMember(Name = "sessionId")]
        public long SessionId { get; set; }
    }
}
