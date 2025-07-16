using OpenMarketingApi.Interfaces.Common;

namespace OpenMarketingApi.Models.Player.Contact
{
    public class PlayerContactsPUT : IServiceAction
    {
        public ContactDetailRef? contactDetailEmail { get; set; }
        public ContactDetailRef? contactDetailPhone { get; set; }
        public ContactDetailRef? contactDetailMobile { get; set; }
        public ContactDetailRef? contactDetailFax { get; set; }
        public ContactDetailRef? contactDetailMailing { get; set; }
        public ContactTypeRef? PreferredContactMean { get; set; }
        public bool? BadOrMissingAddress { get; set; }
        public bool? MailsCollectedAtCasino { get; set; }


        public DateTime? LastUpdatedTimestamp { get; set; }
    }
}
