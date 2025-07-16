using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace OpenTableApi.Models
{
    [DataContract]
    public partial class PlayerSessionCommonInfo
    {
        [Required]
        [DataMember(Name = "sessionStartTimeUTC")]
        public DateTime SessionStartTimeUTC { get; set; }

        [Required]
        [DataMember(Name = "sessionEndTimeUTC")]
        public DateTime SessionEndTimeUTC { get; set; }

        [Required]
        [DataMember(Name = "pauseTimeSeconds")]
        public int PauseTimeSeconds { get; set; }

        [Required]
        [DataMember(Name = "numberOfGames")]
        public int NumberOfGames { get; set; }
        
        [Required]
        [DataMember(Name = "drop")]
        public decimal Drop { get; set; }

        [Required]
        [DataMember(Name = "averageBet")]
        public decimal AverageBet { get; set; }

        [Required]
        [DataMember(Name = "floatVariation")]
        public decimal FloatVariation { get; set; }

        [DataMember(Name = "dropDetails")]
        public DropDetail[]? DropDetails { get; set; }
    }
}