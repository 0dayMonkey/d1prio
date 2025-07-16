namespace OpenMarketingApi.Models.Player.Document
{
    public class PlayerDocumentsPicturesResponse
    {
        [DataMember(Name = "userId")]
        public string UserId { get; set; }

        [DataMember(Name = "picture")]
        public byte[]? Picture { get; set; }

        [DataMember(Name = "location")]
        public Location Location { get; set; }

        [DataMember(Name = "lastUpdatedTimestamp")]
        public DateTime? LastUpdatedTimestamp { get; set; }

        [DataMember(Name = "status")]
        public CrcStatus Status { get; set; }
    }
}
