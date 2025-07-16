using OpenPromoApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPromoApi.Models
{
    public class PromotionResponse
    {
        public string Id { get; set; }
        public PromotionStatus Status { get; set; }
        public List<int>? AllowedCasinoIds { get; set; }
        public string Title { get; set; }
        public string PlayerId { get; set; }
        public PromotionType PromotionType { get; set; }
        public double PromotionValue { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? LastUsageDate { get; set; }
        public string? LastUpdatedBy { get; set; }
        public bool? IsCurrentlyRedeemable { get; set; }
        public bool? IsRequiringPinValidation { get; set; }
    }
}
