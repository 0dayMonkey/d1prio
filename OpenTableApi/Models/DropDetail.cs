using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OpenTableApi.Models
{
    [DataContract]
    public class DropDetail
    {
        [Required]
        [DataMember(Name = "tender")]
        public TenderType Tender { get; set; }

        [Required]
        [DataMember(Name = "amount")]
        public decimal Amount { get; set; }
    }
}