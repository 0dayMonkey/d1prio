using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenMarketingApi.Models.Player.Comment
{
    public class DiffusionResponse
    {
        public Diffusion Domain { get; set; }
        public bool? IsOrigin { get; set; }
        public bool? IsNoOperationAllowed { get; set; }
        public bool? IsReadMandatory { get; set; }
        public bool? IsSignalingMandatory { get; set; }
        public bool? IsDeleted { get; set; }
        public string? DeletionUser { get; set; }
        public string CreationUser { get; set; }
        public DateTime? DeletionDate { get; set; }
        public DateTime? CreationTimestamp { get; set; }
    }
}
