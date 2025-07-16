namespace OpenMarketingApi.Models.Player.Contact
{
    public class PlayerContactsResponse
    {
        public ContactDetailRef? ContactDetailEmail { get; set; }
        public ContactDetailRef? ContactDetailPhone { get; set; }
        public ContactDetailRef? ContactDetailMobile { get; set; }
        public ContactDetailRef? ContactDetailFax { get; set; }
        public ContactDetailRef? ContactDetailMailing { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ContactTypeRef? PreferredContactMean { get; set; }
        public bool? BadOrMissingAddress { get; set; }

        public bool? MailsCollectedAtCasino { get; set; }
    }
}
