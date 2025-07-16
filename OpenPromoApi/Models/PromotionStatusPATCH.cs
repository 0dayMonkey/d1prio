using OpenPromoApi.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenPromoApi.Models
{
    public class PromotionStatusPATCH
    {
        public string? EgmToCredit { get; set; }
        public int CasinoId { get; set; }
        public PromotionStatusPatch Status {  get; set; } 
    }
}
