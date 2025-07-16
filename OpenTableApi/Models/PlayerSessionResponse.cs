using ApiTools.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace OpenTableApi.Models
{
    public class PlayerSessionResponse : PlayerSessionCommonInfo
    {
        [Required]
        [DataMember(Name = "sessionId")]
        public long SessionId { get; set; }

        [Required]
        [JsonConverter(typeof(DateOnlyConverter))]
        [DataMember(Name = "sessionDate")]
        public DateTime SessionDate { get; set; }

        [Required]
        [DataMember(Name = "tableCode")]
        public string TableCode { get; set; }

        [Required]
        [DataMember(Name = "siteID")]
        public int SiteID { get; set; }

        [Required]
        [DataMember(Name = "playerID")]
        public string PlayerID { get; set; }

        [Required]
        [DataMember(Name = "currencyCode")]
        public string CurrencyCode { get; set; }

        [Required]
        [DataMember(Name = "earnedPoints")]
        public decimal EarnedPoints { get; set; }

        [Required]
        [DataMember(Name = "handle")]
        public decimal Handle { get; set; }
    }
}
