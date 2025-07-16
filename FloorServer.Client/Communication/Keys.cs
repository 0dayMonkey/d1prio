#region

using FloorServer.Client.Boss;

#endregion

namespace FloorServer.Client.Communication
{
    public class Keys
    {
        public static readonly string SEQ = "SEQ";
        public static readonly string SEQ_NBR = "SEQ_NBR";
        public static readonly string SEQQR = "SEQQR";
        public static readonly string PL_CARD_NR = "PL_CARD_NR";
        public static readonly string CARD_MEDIA_TYPE = "CARD_MEDIA_TYPE";
        public static readonly string PL_HOME_CASINO = "PL_HOME_CASINO";
        public static readonly string SM_SERNR = "SM_SERNR";
        public static readonly string SM_PROTOCOL_VERSION = "SM_PROTOCOL_VERSION";
        public static readonly string AKTIV = "aktiv";
        public static readonly string AUTH_OWNER_ID = "AUTH_OWNER_ID";


        //STAR|Marketing Keys
        public static readonly string PL_LANGUAGE = "PL_LANGUAGE";
        public static readonly string PL_NAME = "PL_NAME";
        public static readonly string PL_NUMBER = "PL_NUMBER";
        public static readonly string PL_FLAGS_CNT = "PL_FLAGS_CNT";
        public static readonly string PL_FIRST_NAME = "PL_FIRST_NAME";
        public static readonly string PL_LEVEL = "PL_LEVEL";
        public static readonly string PL_LEVEL_CNT = "PL_LEVEL_CNT";
        public static readonly string PL_HOME_TOWN = "PL_HOME_TOWN";
        public static readonly string PL_SEX = "PL_SEX";
        public static readonly string PL_HOST = "PL_HOST";
        public static readonly string PL_TITLE = "PL_TITLE";
        public static readonly string PL_POINTS = "PL_POINTS";
        public static readonly string POINTS_TODAY = "POINTS_TODAY";
        public static readonly string PL_MULTIPLIER = "PL_MULTIPLIER";
        public static readonly string PL_SEGMENTS = "PL_SEGMENTS";
        public static readonly string LAST_VISIT = "LAST_VISIT";
        public static readonly string CNT_VISIT = "CNT_VISIT";
        public static readonly string PL_FLAGS = "PL_FLAGS";
        public static readonly string CASINO = "CASINO";
        public static readonly string SM_GRP_BOSS = "SM_GRP_BOSS";
        public static readonly string SM_GRP_HOST = "SM_GRP_HOST";
        public static readonly string SM_GRP_TYPE = "SM_GRP_TYPE";
        public static readonly string SM_GRP_NAME = "SM_GRP_NAME";
        public static readonly string SM_GRP_ID = "SM_GRP_ID";
        public static readonly string SM_PROM_MUL = "SM_PROM_MUL";
        public static readonly string SM_POINTS = "SM_POINTS";
        public static readonly string END_TIME = "END_TIME";
        public static readonly string EXPIRED = "EXPIRED";
        public static readonly string UNKNOWN = "UNKNOWN";

        // STAR|Marketing Welcome Credits Keys
        public static readonly string WC_CASH_AMOUNT = "WC_CASH_REQUIRED";
        public static readonly string WC_PROMO_AMOUNT = "WC_PROMO_AVAILABLE";
        public static readonly string WC_OFFERED_AMOUNT = "WC_PROMO_OFFERED";
        public static readonly string WC_PROMO_NAME = "WC_PROMOTION_NAME";
        public static readonly string WC_RESULT = "WC_RESULT";
        public static readonly string WC_CASH_CENTS_BALANCE = "SBC_CASH_CENTS_BALANCE";
        public static readonly string WC_PROMO_CENTS_BALANCE = "SBC_PROM_CENTS_BALANCE";

        //STAR|Control Keys
        public static readonly string CL_TC_OWNERID = "CL_TC_OWNERID";
        public static readonly string CL_TR_UCARDID = "CL_TR_UCARDID";
        public static readonly string CL_TC_NEWTYPE = "CL_TC_NEWTYPE";
        public static readonly string CL_TC_OLDTYPE = "CL_TC_OLDTYPE";
        public static readonly string CL_TR_TIMESTAMP = "CL_TR_TIMESTAMP";

        //STAR|JACKPOT Keys
        public static readonly string JP_GROUP = "JP_GROUP";

        //ECT Keys
        public static readonly string ECT_AMOUNT = "ECT_AMOUNT";
        public static readonly string ECT_CARD_NR = "ECT_CARD_NR";
        public static readonly string ECT_CREDIT_TYPE = "ECT_CREDIT_TYPE";
        public static readonly string ECT_DESTINATION = "ECT_DESTINATION";
        public static readonly string ECT_FLAGS = "ECT_FLAGS";
        public static readonly string ECT_HOST_ID = "ECT_HOST_ID";
        public static readonly string ECT_MESSAGE = "ECT_MESSAGE";
        public static readonly string ECT_TIMEOUT = "ECT_TIMEOUT";
        public static readonly string ECT_TRANSFER_ID = "ECT_TRANSFER_ID";
        public static readonly string ECT_RESULT = "ECT_RESULT";
        public static readonly string ECT_ORIGIN = "ECT_ORIGIN";

        //TITO Keys
        public static readonly string TITO_VAL_CFG_MACH_ID = "TITO_VAL_CFG_MACH_ID";
        public static readonly string TITO_VAL_CFG_SEQ_NR = "TITO_VAL_CFG_SEQ_NR";
        public static readonly string TITO_VAL_DATA = "TITO_VAL_DATA";
        public static readonly string TITO_VAL_DATE = "TITO_VAL_DATE";
        public static readonly string TITO_AMOUNT = "TITO_AMOUNT";
        public static readonly string TITO_TICKET_NR = "TITO_TICKET_NR";
        public static readonly string TITO_VAL_TYPE = "TITO_VAL_TYPE";
        public static readonly string TITO_POOL_ID = "TITO_POOL_ID";
        public static readonly string TITO_EXP_DATE = "TITO_EXP_DATE";
        public static readonly string TITO_SYSTEM_ID = "TITO_SYSTEM_ID";
        public static readonly string TITO_RESULT = "TITO_RESULT";
        public static readonly string TITO_CASHOUT_TYPE = "TITO_CASHOUT_TYPE";
        public static readonly string TITO_REDEMPTION_REPLY = "TITO_REDEMPTION_REPLY";
        public static readonly string TITO_REDEMPTION_EXP = "TITO_REDEMPTION_EXP";
        public static readonly string TITO_CFG_LOC = "TITO_CFG_LOC";
        public static readonly string TITO_CFG_ADD1 = "TITO_CFG_ADD1";
        public static readonly string TITO_CFG_ADD2 = "TITO_CFG_ADD2";
        public static readonly string TITO_CFG_RESTRICT_TITLE = "TITO_CFG_RESTRICT_TITLE";
        public static readonly string TITO_CFG_DEBIT_TITLE = "TITO_CFG_DEBIT_TITLE";
        public static readonly string TITO_CFG_CASH_EXP = "TITO_CFG_CASH_EXP";
        public static readonly string TITO_CFG_RESTRICT_EXP = "TITO_CFG_RESTRICT_EXP";
        public static readonly string TITO_CFG_CTRL = "TITO_CFG_CTRL";
        public static readonly string TITO_LINK_STATUS = "IVS_LINK";

        public static readonly string PL_POINTS_TEXT = "PLAYER_POINTS";

        public static readonly string CREDITS_PLAYED = "CREDITS_PLAYED";
        public static readonly string CREDITS_CASHED = "CREDITS_CASHED";
        public static readonly string HP_CASHED = "HP_CASHED";
        public static readonly string GAMES_PLAYED = "GAMES_PLAYED";
        public static readonly string EARNED_POINTS = "A_BON";
        //TODO:UPDATE FS TAG WHEN AVAILABLE
        public static readonly string MAX_POINTS = "PL_CAP_AMOUNT";
        public static readonly string LUCK_FACTOR = "PL_LUCK_FACTOR";
        public static readonly string DISABLE_POINT_EARNING = "PL_ENABLE_POINT_NO";

        public static readonly string READ_OFFSET = "READ_OFFSET";
        public static readonly string TERMINATOR = "TERMINATOR";


        public static readonly string ACK_REQU = Tags.ACK_REQU;
        public static readonly string ACT = Tags.ACT;
        public static readonly string ACTIVE = Tags.ACTIVE;
        public static readonly string APPLICATION = Tags.APPLICATION;
        public static readonly string BI = Tags.BI;
        public static readonly string BI1 = Tags.BI1;
        public static readonly string BI10 = Tags.BI10;
        public static readonly string BI100 = Tags.BI100;
        public static readonly string BI20 = Tags.BI20;
        public static readonly string BI200 = Tags.BI200;
        public static readonly string BI5 = Tags.BI5;
        public static readonly string BI50 = Tags.BI50;
        public static readonly string BI500 = Tags.BI500;
        public static readonly string BOSS = Tags.BOSS;
        public static readonly string BUILD = Tags.BUILD;
        public static readonly string CASHOUT_FLAGS = "CASHOUT_FLAGS";
        public static readonly string CASINO_CUR_CODE = Tags.CASINO_CUR_CODE;
        public static readonly string CASINO_CUR_NAME = Tags.CASINO_CUR_NAME;
        public static readonly string CASINO_CUR_SYMBOL = Tags.CASINO_CUR_SYMBOL;
        public static readonly string CASINO_ID = Tags.CASINO_ID;
        public static readonly string CASINO_NAME = Tags.CASINO_NAME;
        public static readonly string CC = Tags.CC;
        public static readonly string CFG_FLAGS = Tags.CFG_FLAGS;
        public static readonly string CHANGED = Tags.CHANGED;
        public static readonly string CI = Tags.CI;
        public static readonly string CISEQ = "CISEQ";
        public static readonly string CL_FLAGS = Tags.CL_FLAGS;
        public static readonly string CLI = Tags.CLI;
        public static readonly string CLO = Tags.CLO;
        public static readonly string CMD = Tags.CMD;
        public static readonly string CMDID = Tags.CMDID;
        public static readonly string CO = Tags.CO;
        public static readonly string COUNT = Tags.COUNT;
        public static readonly string COUNTRY_CODE = Tags.COUNTRY_CODE;
        public static readonly string CR = Tags.CR;
        public static readonly string CR_TIM = Tags.CR_TIM;
        public static readonly string CTC = Tags.CTC;
        public static readonly string DAYS = Tags.DAYS;
        public static readonly string DEF_LANGUAGE = Tags.DEF_LANGUAGE;
        public static readonly string DENOM = Tags.DENOM;
        public static readonly string DETAILS = Tags.DETAILS;
        public static readonly string DST = Tags.DST;
        public static readonly string EMPTY = Tags.EMPTY;
        public static readonly string ERR = Tags.ERR;
        public static readonly string ERROR = Tags.ERROR;
        public static readonly string EVJP_VAL_HIGH = Tags.EVJP_VAL_HIGH;
        public static readonly string EVJP_VAL_LOW = Tags.EVJP_VAL_LOW;
        public static readonly string EXC = Tags.EXC;
        public static readonly string EXC_NUM = Tags.EXC_NUM;
        public static readonly string EXTERNAL_USER_ID = Tags.EXTERNAL_USER_ID;
        public static readonly string FIELDS = Tags.FIELDS;
        public static readonly string FIELDS_REQU = Tags.FIELDS_REQU;
        public static readonly string FS = Tags.FS;
        public static readonly string FSCONN = Tags.FSCONN;
        public static readonly string GM = Tags.GM;
        public static readonly string HASH = Tags.HASH;
        public static readonly string HIT_INSTANCE = Tags.HIT_INSTANCE;
        public static readonly string HIT_JP_GROUP = Tags.HIT_JP_GROUP;
        public static readonly string HIT_LOC_VALUE = Tags.HIT_LOC_VALUE;
        public static readonly string HIT_SMID = Tags.HIT_SMID;
        public static readonly string HIT_TIME = Tags.HIT_TIME;
        public static readonly string HIT_VALUE = Tags.HIT_VALUE;
        public static readonly string ID_RANGE_HIGH = Tags.ID_RANGE_HIGH;
        public static readonly string ID_RANGE_LOW = Tags.ID_RANGE_LOW;
        public static readonly string INFO = Tags.INFO;
        public static readonly string JD_DATA1 = "JD_DATA1";
        public static readonly string JD_DATA2 = "JD_DATA2";
        public static readonly string JPM = Tags.JPM;
        public static readonly string JPM_GROUP = Tags.JPM_GROUP;
        public static readonly string JPM_IM_DISPLAY = Tags.JPM_IM_DISPLAY;
        public static readonly string JPM_LEV_MASK = Tags.JPM_LEV_MASK;
        public static readonly string JPM_SMDBID = Tags.JPM_SMDBID;
        public static readonly string LOCID = Tags.LOCID;
        public static readonly string LOCNAME = Tags.LOCNAME;
        public static readonly string MAXBET = Tags.MAXBET;
        public static readonly string MC = Tags.MC;
        public static readonly string MDCID = Tags.MDCID;
        public static readonly string MEMBERS = Tags.MEMBERS;
        public static readonly string MSG_REQU = Tags.MSG_REQU;
        public static readonly string MYNAME = Tags.MYNAME;
        public static readonly string NAME = Tags.NAME;
        public static readonly string NO_STAFF_EXP = Tags.NO_STAFF_EXP;
        public static readonly string QUERY = Tags.QUERY;
        public static readonly string PRINTER_ERRCODE = "PRINTER_ERRCODE";
        public static readonly string READ_FIELDS = Tags.READ_FIELDS;
        public static readonly string READ_ID = Tags.READ_ID;
        public static readonly string READ_NAME = Tags.READ_NAME;
        public static readonly string READ_REPEAT = Tags.READ_REPEAT;
        public static readonly string READ_TIME = Tags.READ_TIME;
        public static readonly string READ_WHERE = Tags.READ_WHERE;
        public static readonly string RECV = Tags.RECV;
        public static readonly string REQ = "REQ";
        public static readonly string RPL = Tags.RPL;
        public static readonly string SM_IF_TYPE = Tags.SM_IF_TYPE;
        public static readonly string SM_STATUS = "SM_STATUS";
        public static readonly string MDC_STATUS = "MDC_STATUS";
        public static readonly string CL_SESSION_ST = "CL_SESSION_ST";
        public static readonly string SC_CARDNR = "SC_CARDNR";
        public static readonly string SM_MANUF = Tags.SM_MANUF;
        public static readonly string SM_VFY_STR = Tags.SM_VFY_STR;
        public static readonly string SMDBID = Tags.SMDBID;
        public static readonly string SMHWID = Tags.SMHWID;
        public static readonly string SMNAME = Tags.SMNAME;
        public static readonly string SRC = Tags.SRC;
        public static readonly string START = Tags.START;
        public static readonly string START_TIME = "START_TIME";
        public static readonly string STATUS = Tags.STATUS;
        public static readonly string STOP = Tags.STOP;
        public static readonly string SYNC_ID = Tags.SYNC_ID;
        public static readonly string TABLE = Tags.TABLE;
        public static readonly string TEMPLATENAME = Tags.TEMPLATENAME;
        public static readonly string TI = Tags.TI;
        public static readonly string TI_START = Tags.TI_START;
        public static readonly string TIME_START_SD = "TIME_START_SD";
        public static readonly string TIME_END_SD = "TIME_END_SD";
        public static readonly string TIME_START_LD = "TIME_START_LD";
        public static readonly string TIME_START_CB = "TIME_START_CB";
        public static readonly string TIME_END_CB = "TIME_END_CB";
        public static readonly string TIME_END_LD = "TIME_END_LD";
        public static readonly string TIME_START_BB = "TIME_START_BB";
        public static readonly string TIME_END_BB = "TIME_END_BB";
        public static readonly string TO = Tags.TO;
        public static readonly string TV = Tags.TV;
        public static readonly string TYP = Tags.TYP;
        public static readonly string TZ = Tags.TZ;
        public static readonly string TRANSFER_DESTINATION = "TRANSFER_DESTINATION";
        public static readonly string TRANSFER_MONEY_TYPE = "TRANSFER_MONEY_TYPE";
        public static readonly string VAL_HIGH = Tags.VAL_HIGH;
        public static readonly string VAL_LOW = Tags.VAL_LOW;
        public static readonly string VALUES = Tags.VALUES;
        public static readonly string VERSION = Tags.VERSION;
        public static readonly string WAP_UPDATE_INTVL = Tags.WAP_UPDATE_INTVL;
        public static readonly string WHERE = Tags.WHERE;
        public static readonly string WHERE_REQU = Tags.WHERE_REQU;
        public static readonly string REEL_NR = "REEL_NR";
        public static readonly string TIME_SM_PWR_ON = "TIME_SM_PWR_ON";
        public static readonly string TIME_SM_PWR_OFF = "TIME_SM_PWR_OFF";
        public static readonly string TIME_SM_CON_OFF = "TIME_SM_CON_OFF";
        public static readonly string TIME_SM_CON_ON = "TIME_SM_CON_ON";
        public static readonly string CL_SAM_CERT_MODE = "CL_SAM_CERT_MODE";
        public static readonly string CL_SAM_NR = "CL_SAM_NR";
        public static readonly string CL_TR_NR2_P1 = "CL_TR_NR2_P1";
        public static readonly string CL_TR_NR2_P3 = "CL_TR_NR2_P3";
        public static readonly string CL_TR_SMID = "CL_TR_SMID";
        public static readonly string SAM_PURSE1 = "SAM_PURSE1";
        public static readonly string SAM_PURSE3 = "SAM_PURSE3";
        public static readonly string SCLR_ID = "SCLR_ID";
        public static readonly string HCLR_ID = "HCLR_ID";

        public static readonly string CL_PURSE1_BAL = "CL_PURSE1_BAL";
        public static readonly string CL_PURSE2_BAL = "CL_PURSE2_BAL";
        public static readonly string CL_PURSE3_BAL = "CL_PURSE3_BAL";
        public static readonly string CL_PURSE4_BAL = "CL_PURSE4_BAL";
        public static readonly string CL_PURSE1_BAL_EXP = "CL_PURSE1_BAL_EXP";
        public static readonly string CL_PURSE3_BAL_EXP = "CL_PURSE3_BAL_EXP";

        public static readonly string CL_TR_NR = "CL_TR_NR";
        public static readonly string CL_TR_NR2 = "CL_TR_NR2";
        public static readonly string CL_TR_PHASE = "CL_TR_PHASE";
        public static readonly string CL_TR_PURSE_NR = "CL_TR_PURSE_NR";
        public static readonly string CL_TR_SAMID = "CL_TR_SAMID";
        public static readonly string CL_TR_STATE = "CL_TR_STATE";
        public static readonly string CL_TR_TYPE = "CL_TR_TYPE";
        public static readonly string CL_TR_ERR = "CL_TR_ERR";

        public static readonly string TIME_START_FD = "TIME_START_FD";
        public static readonly string TIME_END_FD = "TIME_END_FD";
        public static readonly string CL_SAM_MSG = "CL_SAM_MSG";
        public static readonly string HO_CHANGE = "HO_CHANGE";
        public static readonly string HO_OLD = "HO_OLD";
        public static readonly string PLIP_CODE = "PLIP_CODE";

        public static readonly string CL_TR_NR_P1 = "CL_TR_NR_P1";
        public static readonly string CL_TR_NR_P2 = "CL_TR_NR_P2";
        public static readonly string CL_TR_NR_P3 = "CL_TR_NR_P3";
        public static readonly string CL_TR_NR_P4 = "CL_TR_NR_P4";
        public static readonly string CL_TR_NR_P1_EXP = "CL_TR_NR_P1_EXP";
        public static readonly string CL_TR_NR_P3_EXP = "CL_TR_NR_P3_EXP";

        public static readonly string CARD_LOCK_REASON = "CARD_LOCK_REASON";
        public static readonly string CARD_REJECT_REASON = "CARD_REJECT_REASON";
        public static readonly string CL_TR_AMT = "CL_TR_AMT";
        public static readonly string CL_TR_DIRECTION = "CL_TR_DIRECTION";
        public static readonly string HIT_CASINO_ID = "HIT_CASINO_ID";
        public static readonly string HIT_JP_NAME = "HIT_JP_NAME";
        public static readonly string HIT_JP_TYPE = "HIT_JP_TYPE";
        public static readonly string HIT_LEVEL = "HIT_LEVEL";
        public static readonly string HIT_PIN = "HIT_PIN";
        public static readonly string HIT_SOURCE = "HIT_SOURCE";
        public static readonly string HIT_STAGE = "HIT_STAGE";
        public static readonly string HIT_TEXT = "HIT_TEXT";
        public static readonly string HIT_THRESHOLD = "HIT_THRESHOLD";
        public static readonly string JP_CASINO_ID = "JP_CASINO_ID";
        public static readonly string PAYOUT_CARD = "PAYOUT_CARD";
        public static readonly string PAYOUT_METHOD = "PAYOUT_METHOD";
        public static readonly string REMOTE_EXC = "REMOTE_EXC";
        public static readonly string WRAP_DELTA = "WRAP_DELTA";
        public static readonly string WRAP_METER_VALUE_CURR = "WRAP_METER_VALUE_CURR";
        public static readonly string WRAP_METER_VALUE_PREV = "WRAP_METER_VALUE_PREV";
        public static readonly string CC_VALUE = "CC_VALUE";
        public static readonly string JP_HIT_CREDITS = "JP_HIT_CREDITS";
        public static readonly string JP_HIT_FLAGS = "JP_HIT_FLAGS";
        public static readonly string JP_HIT_LEVEL = "JP_HIT_LEVEL";
        public static readonly string JP_HIT_GROUP = "JP_HIT_GROUP";
        public static readonly string JP_HIT_PARTIAL_PAY = "JP_HIT_PARTIAL_PAY";
        public static readonly string SAS_PROGRESSIVE_AMOUNT = "SAS_PROGRESSIVE_AMOUNT";
        public static readonly string SAS_PROGRESSIVE_COUNT = "SAS_PROGRESSIVE_COUNT";

        public static readonly string TICKET_AMOUNT = "TICKET_AMOUNT";
        public static readonly string TICKET_ID = "TICKET_ID";
        public static readonly string TICKET_VALIDATION = "TICKET_VALIDATION";
        public static readonly string TITO_CFG_RESULT = "TITO_CFG_RESULT";
        public static readonly string SVC_CODE = "SVC_CODE";
        public static readonly string SM_EPROM_RES = "SM_EPROM_RES";
        public static readonly string HIT_CMD_GROUP = "HIT_CMD_GROUP";
        public static readonly string HIT_CMD_GROUP_32 = "HIT_CMD_GROUP_32";
        public static readonly string HIT_CMD_INSTANCE = "HIT_CMD_INSTANCE";
        public static readonly string HIT_CMD_VALUE_64 = "HIT_CMD_VALUE_64";        
        public static readonly string HIT_CMD_VALUE = "HIT_CMD_VALUE";
        public static readonly string PAY_TO_CARD = "PAY_TO_CARD";

        public static readonly string MYSTERY_RESET_METHOD = "MYSTERY_RESET_METHOD";
        public static readonly string MDC_PURSE_IRS = "MDC_PURSE_IRS";
        public static readonly string MDC_PURSE_NPR = "MDC_PURSE_NPR";
        public static readonly string MDC_PURSE_PR = "MDC_PURSE_PR";

        public static readonly string CCHP_AMOUNT_CASH = "CCHP_AMOUNT_CASH";
        public static readonly string CCHP_AMOUNT_NONCASH = "CCHP_AMOUNT_NONCASH";
        public static readonly string CCHP_END_TIME = "CCHP_END_TIME";
        public static readonly string CCHP_START_TIME = "CCHP_START_TIME";
        public static readonly string CCHP_REASON = "CCHP_REASON";

        public static readonly string SEED = "SEED";
        public static readonly string SIGNATURE = "SIGNATURE";
    }

    public class ProtocolKeys
    {
        public const string LIVE_MSGID = "LIVE_MSGID";
        public const string LIVE_MSG_CARDNR = "LIVE_MSG_CARDNR";
        public const string LIVE_MSG_EXPIRY = "LIVE_MSG_EXPIRY";
        public const string LIVE_MSG_FLAGS = "LIVE_MSG_FLAGS";
        public const string LIVE_TEXT = "LIVE_TEXT";
        public const string LIVE_MSG_HOSTID = "LIVE_MSG_HOSTID";
        public const string LIVE_MSG_LANGUAGE_CODE = "LANGUAGE_CODE";
        public const string LIVE_MSG_RESULT = "LIVE_MSG_RESULT";
        public const string LIVE_MSG_BUTTON2_TEXT = "LIVE_MSG_BUTTON2_TEXT";
        public const string LIVE_MSG_BUTTON_TEXT = "LIVE_MSG_BUTTON_TEXT";
        public const string LIVE_MSG_DURATION = "LIVE_MSG_DURATION";
        public const string LIVE_MSG_PRIORITY = "LIVE_MSG_PRIORITY";
        public const string LIVE_MSG_RESOURCES = "LIVE_MSG_RESOURCES";
        public const string LIVE_MSG_TEMPLATE_ID = "LIVE_MSG_TEMPLATE_ID";
        public const string LIVE_MSG_WITH_ANSWER = "LIVE_MSG_WITH_ANSWER";
    }
}