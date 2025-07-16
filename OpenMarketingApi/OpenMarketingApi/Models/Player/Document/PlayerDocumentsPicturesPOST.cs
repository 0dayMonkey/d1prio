namespace OpenMarketingApi.Models.Player.Document
{
    public class PlayerDocumentsPicturesPOST
    {
        [Required]
        public byte[] Picture { get; set; }
        [Required]
        public Location Location { get; set; }
        public DateTime? LastUpdatedTimestamp { get; set; }

    }
}
