namespace OpenMarketingApi.Models.Player.Comment
{
    public partial class PlayerCommentResponse
    {
        public string Id { get; set; } = null!;
        public string Text { get; set; } = null!;
        public string CreationUserId { get; set; }
        public List<DiffusionResponse> DiffusionList { get; set; }
        public string? LastUpdatedUserId { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public DateOnly GamingDayStart { get; set; }
        public DateOnly? GamingDayEnd { get; set; }
        public DateTime LastUpdatedTimestamp { get; set; }
    }
}
