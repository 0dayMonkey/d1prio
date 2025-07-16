namespace FloorServer.Client.Communication
{
    public class CommandKeys
    {
        public const string ACK = "ACK";
        public const string RENAME = "RENAME";

        public const string CMD_HELLO = "100";
        public const string CMD_LIVE_MSG = "1600";
        public const string CMD_REGISTER = "800";
        public const string CMD_UNREGISTER = "802";
        public const string CMD_PERSIST = "804";
        public const string CMD_DEL_PERSIST = "805";
        public const string CMD_EJECT_CARD = "321";
        public const string CMD_ADD_READING = "600";
        public const string CMD_DEL_READING = "601";
        public const string CMD_CLR_READING = "602";

        public const string CMD_UPD_PLAYER = "1300";

        public const string CMD_POINTS_UPDATE = "662";

        //TITO Commands
        public const string CMD_TICKET_REDEMPTION = "2112";
        public const string CMD_TICKET_ISSUE = "2113";
        public const string CMD_TICKET_CFG_UPDATE = "2110";
        public const string CMD_TICKET_LINK_UPDATE = "130";
        public const string CMD_TICKET_VALIDATION_PARAMS = "2111";

        //ECT Commands
        public const string CMD_ECT_START = "3000";

        //HAPPY HOUR Commands
        public const string CMD_CREATE_GROUP = "350";
        public const string CMD_DELETE_GROUP = "352";
        public const string CMD_ADD_SM_TO_GROUP = "360";
        public const string CMD_REMOVE_SM_FROM_GROUP = "362";

        //EGM commands
        public const string LOCK_SM = "303";
        public const string UNLOCK_SM = "304";
        public const string DESTROY_CONFIG = "305";
        public const string RESET_MDC = "306";
        public const string CASHOUT_SM = "322";
        public const string CMD_CASHIN_DISABLE = "323";
        public const string CMD_CASHIN_ENABLE = "324";

        // Welcome Credits
        public const string CMD_WELCOME_CREDITS_ELIGIBILITY = "3050";
        public const string CMD_WELCOME_CREDITS_OFFER_RESPONSE = "3052";
    }
}
