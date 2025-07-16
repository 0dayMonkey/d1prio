using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Player.Comment
{
    public partial class PlayerCommentPOST : IServiceAction
    {
        public string? Id { get; set; } = null!;
        public string Text { get; set; } = null!;
        public List<DiffusionPOSTPUT> DiffusionList { get; set; }
        public DateTime? CreationTimestamp { get; set; }
        public DateOnly GamingDayStart { get; set; }
        public DateOnly? GamingDayEnd { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
