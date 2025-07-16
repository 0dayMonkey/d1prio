namespace FloorServer.Client.Communication
{
    public class ExceptionKeys
    {
        public const long ALL = 0;
        public const long EXC_FS_CONNECTION = 10;
        public const long EXC_FS_RECONNECTION = 11;
        public const long EXC_NEW_SESSION = 3000;
        public const long EXC_END_SESSION = 3001;
        public const long EXC_SM_READING = 2001;
        public const long EXC_READING_DONE = 2003;
        public const long EXC_LIVEMSG_RESULT = 3300;
        
        /// <summary>
        /// Gaming machine disabled
        /// </summary>
        public const long EXC_SM_LOCKED = 1031;
        
        /// <summary>
        /// Gaming machine enabled
        /// </summary>
        public const long EXC_SM_UNLOCKED = 1030;

        //ECT Exceptions
        public const long ECT_CTC = 1092;        

        //Ticket specific exceptions
        public const long EXC_CFG_VERIFY_OK = 9007;
        public const long EXC_SM_DELETED = 1004;
        public const long EXC_TITO_CFG_REQ = 4814;
        public const long EXC_TITO_TICKET_OUT = 4815;
        public const long EXC_TITO_VAL_CFG_REQ = 4803;
        public const long EXC_TITO_REDEMPTION_REQ = 4816;
        public const long EXC_REDEMPTION_COMPLETE = 4806;
        /// <summary>
        /// Billbox removed from gaming machine.
        /// </summary>
        public const long EXC_BILLBOX_OUT = 1020;
        public const long EXC_TITO_SYSTEM_VAL_REQ = 4817;
        public const long EXC_TITO_SYSTEM_VAL_ERROR = 4818;
        public const long EXC_TITO_CFG_UPDATED = 4805;
        public const long EXC_CASH_CRD_CREATED = 4012;
        public const long EXC_TICKET_IN = 4800;

        // Game events
        public const long EXC_GAME_START = 1998;
        public const long EXC_GAME_END = 1999;


        //Session events
        public const long PL_INTERIM_RECORD = 3006;


        //Hot player
        public const long ST_HP_TI = 3200;
        public const long ST_HP_GM = 3100;

        public const long ST_HP_TI_OFF = 3201;
        public const long ST_HP_GM_OFF = 3101;

        /// <summary>
        /// Credit cancel handpay on gaming machine
        /// </summary>
        public const long EXC_CREDITS_CANCELED = 1100;

        //Jackpot 
        public const long EXC_FS_JACKPOT = 1101;

        /// <summary>
        /// (Progressive) jackpot hit on gaming machine
        /// </summary>
        public const long EXC_PROGJP_MVAL = 1102;
                
        /// <summary>
        /// Cancelled Credits or jackpot reset key turned on machine
        /// </summary>
        public const long JP_RESET = 1110;
        
        // Mystery payout
        public const long EXC_MT_PAYOUT = 1097;

        /// <summary>
        /// Bill accepted
        /// </summary>
        public const long EXC_SM_BILL_ACC = 143;

        // CaWa
        public const long SBC_COMMIT_TRANSFER = 257;
        public const long CL_TRANSFER_COMPLETE = 4301;
        
        // Welcome Credits
        public const long EXC_WC_OFFER_RESPONSE = 3051;

        // No Money In
        public const long EXC_NO_MONEY_IN_ENABLED = 1044;
        public const long EXC_NO_MONEY_IN_DISABLED = 1043;

        // Already existing floor events

        /// <summary>
        /// Gaming machine is connected to MDC again
        /// Tags:
        /// TIME_SM_CON_OFF (Time when gaming machine disconnected)
        /// TIME_SM_CON_ON (Time when gaming machine connected)
        /// </summary>
        public const long EXC_SM_CONNECT = 120;

        /// <summary>
        /// Gaming machine is disconnected from MDC
        /// Tags:
        /// TIME_SM_CON_OFF (Time when gaming machine disconnected)
        /// TIME_SM_CON_ON (Time when gaming machine connected)
        /// </summary>
        public const long EXC_SM_DISCON = 119;

        /// <summary>
        /// Gaming machine power is on again
        /// Tags:
        /// TIME_SM_PWR_OFF (Time of gaming machine power-off)
        /// TIME_SM_PWR_ON (Time of gaming machine power-up)
        /// </summary>
        public const long EXC_SM_POWER_ON = 116;

        /// <summary>
        /// Gaming machine power is off
        /// Tags:
        /// TIME_SM_PWR_ON (Time of gaming machine power-up)
        /// </summary>
        public const long EXC_SM_POWER_OFF = 115;

        /// <summary>
        /// SM service start - SM door
        /// Tags:
        /// TIME_END_SD (End time of gaming machine door opening)
        /// TIME_START_SD (Start time of gaming machine door opening)
        /// </summary>
        public const long EXC_SM_SVC_SD_STRT = 100;

        /// <summary>
        /// SM service end - SM door
        /// Tags:
        /// TIME_END_SD (End time of gaming machine door opening)
        /// TIME_START_SD (Start time of gaming machine door opening)
        /// </summary>
        public const long EXC_SM_SVC_SD_END = 101;

        /// <summary>
        /// SM service auto end - SM door
        /// Tags:
        /// TIME_END_SD (End time of gaming machine door opening)
        /// TIME_START_SD (Start time of gaming machine door opening)
        /// </summary>
        public const long EXC_SM_SVC_SD_AEND = 102;

        /// <summary>
        /// SM door closed without authorization (no technician card inserted)
        /// Tags:
        /// TIME_END_SD (End time of gaming machine door opening)
        /// TIME_START_SD (Start time of gaming machine door opening)
        /// </summary>
        public const long EXC_SM_AL_SD_END = 107;

        /// <summary>
        /// SM service start - CB door
        /// Tags:
        /// TIME_START_CB (Start time of cashbox door opening)
        /// </summary>
        public const long EXC_SM_SVC_CB_STRT = 103;

        /// <summary>
        /// Unauthorized opening of cashbox door
        /// Tags:
        /// TIME_END_CB (End time of cashbox door opening)
        /// TIME_START_CB (Start time of cashbox door opening)
        /// </summary>
        public const long EXC_SM_AL_CB_STRT = 108;

        /// <summary>
        /// SM service end - CB door
        /// Tags:
        /// TIME_END_CB (End time of cashbox door opening)
        /// TIME_START_CB (Start time of cashbox door opening)
        /// </summary>
        public const long EXC_SM_SVC_CB_END = 104;

        /// <summary>
        /// Service auto end - CB door
        /// Tags:
        /// TIME_END_CB (End time of cashbox door opening)
        /// TIME_START_CB (Start time of cashbox door opening)
        /// </summary>
        public const long EXC_SM_SVC_CB_AEND = 105;

        /// <summary>
        /// Unauthorized closing of cashbox door
        /// Tags:
        /// TIME_END_CB (End time of cashbox door opening)
        /// TIME_START_CB (Start time of cashbox door opening)
        /// </summary>
        public const long EXC_SM_AL_CB_END = 109;

        /// <summary>
        /// Billbox door closed with service card
        /// Tags:
        /// TIME_END_BB (End time of billbox door opening)
        /// TIME_START_BB (Start time of billbox door opening)
        /// </summary>
        public const long EXC_SM_SVC_BB_END = 112;

        /// <summary>
        /// Billbox door closed without service card
        /// Tags:
        /// TIME_END_BB (End time of billbox door opening)
        /// TIME_START_BB (Start time of billbox door opening)
        /// </summary>
        public const long EXC_SM_AL_BB_END = 128;

        /// <summary>
        /// Gaming machine reel tilt (REEL_NR only sent with CPX)
        /// Tags:
        /// REEL_NR (Reel number
        /// </summary>
        public const long EXC_SM_TILT_REEL = 157;

        /// <summary>
        /// End of EGM tilt state has been detected by SMIB.
        /// </summary>
        public const long EXC_SM_TILT_END = 197;

        /// <summary>
        /// Gaming machine's audit menu has been entered.
        /// </summary>
        public const long EXC_SVC_DISPLAY_METERS_START = 5200;

        /// <summary>
        /// Gaming machine's audit menu has been left.
        /// </summary>
        public const long EXC_SVC_DISPLAY_METERS_END = 5201;

        /// <summary>
        /// Logic door opened
        /// Tags:
        /// TIME_START_LD (Start time of logic door opening)
        /// </summary>
        public const long EXC_SM_AL_LD_STRT = 1022;

        /// <summary>
        /// Logic door of the gaming machine has been opened in service mode.
        /// </summary>
        public const long EXC_SM_SVC_LD_START = 1058;

        /// <summary>
        /// Logic door of the gaming machine has been closed in service mode.
        /// </summary>
        public const long EXC_SM_SVC_LD_END = 1059;

        /// <summary>
        /// Clearance card has been inserted
        /// </summary>
        public const long EXC_CLR_CARDIN = 5005;

        /// <summary>
        /// enter service stat after plip
        /// </summary>
        public const long EXC_PLIP_SESSIONSTART = 5100;

        /// <summary>
        /// Clearance card has been removed
        /// </summary>
        public const long EXC_CLR_CARDOUT = 5006;

        /// <summary>
        /// exit service state after plip or timeout
        /// </summary>
        public const long EXC_PLIP_SESSIONEND = 5101;

        /// <summary>
        /// Billbox installed in gaming machine. Comment: coded in SMIF and in ASP
        /// </summary>
        public const long EXC_BILLBOX_IN = 1021;

        /// <summary>
        /// Communication with the gaming machine is ok (in NSW, no tag is sent)
        /// Tags:
        /// JD_DATA2 (time since when the error was)
        /// </summary>
        public const long EXC_SMCOMM_OK = 1011;

        /// <summary>
        /// Gaming machine is operating (in NSW, no tag is sent)
        /// </summary>
        public const long EXC_SM_OPERATING = 1013;

        /// <summary>
        /// Gaming machine is not operating.
        /// </summary>
        public const long EXC_SM_NOT_OPERATING = 1012;

        /// <summary>
        /// Bill validator ok again
        /// </summary>
        public const long EXC_SM_BV_OK = 169;

        /// <summary>
        /// All information required for IVS received from gaming machine. This does not include valid meters.	
        /// </summary>
        public const long EXC_SM_INFO_COMPLETE = 1070;

        /// <summary>
        /// SAM unusable (e.g. invalid keys)
        /// </summary>
        public const long EXC_SAM_DEFECT = 4036;

        /// <summary>
        /// SAM initialized, sent with SAM_SERNO and purses
        /// Tags:
        /// CL_SAM_CERT_MODE (Mode used by SAM to calculate certificates. Current possible values: "R" ... regular, "S" ... strong)
        /// CL_SAM_NR (SAM ID number)
        /// CL_TR_NR2_P1 (Transaction number of other card (SAM/U_CARD) for purse 1)
        /// CL_TR_NR2_P3 (Transaction number of other card (SAM/U_CARD) for purse 3)
        /// CL_TR_SMID (SMHWID for the current transaction)
        /// SAM_PURSE1 (Cents in SAM purse 1)
        /// SAM_PURSE3 (Cents in SAM purse 3)
        /// </summary>
        public const long EXC_CL_SAMINIT = 4015;

        /// <summary>
        /// SAM has been removed
        /// </summary>
        public const long EXC_SAM_REMOVED = 4010;

        /// <summary>
        /// SMA or MDC is connected again Comment: FS generated
        /// </summary>
        public const long EXC_MDC_CONNECT = 118;

        /// <summary>
        /// SMA or MDC is disconnected Comment: FS generated
        /// </summary>
        public const long EXC_MDC_DISCON = 117;

        /// <summary>
        /// SM door opened without authorization (no technician card inserted)
        /// Tags:
        /// TIME_END_SD (End time of gaming machine door opening)
        /// TIME_START_SD (Start time of gaming machine door opening)
        /// </summary>
        public const long EXC_SM_AL_SD_START = 106;
        
        /// <summary>
        /// Logic door closed
        /// Tags:
        /// JD_DATA2 (time of last opening)
        /// TIME_END_LD (End time of logic door opening)
        /// TIME_START_LD (Start time of logic door opening)
        /// </summary>
        public const long EXC_SM_AL_LD_END = 1023;

        /// <summary>
        /// Unauthorized opening of cashbox door
        /// Tags:
        /// TIME_END_CB (End time of cashbox door opening)
        /// TIME_START_CB (Start time of cashbox door opening)
        /// </summary>
        public const long EXC_SM_AL_CB_START = 108;

        /// <summary>
        /// Billbox door opened without service card
        /// Tags:
        /// TIME_END_BB (End time of billbox door opening)
        /// TIME_START_BB (Start time of billbox door opening)
        /// </summary>
        public const long EXC_SM_AL_BB_STRT = 127;

        /// <summary>
        /// Billbox door opened with service card
        /// Tags:
        /// TIME_START_BB (Start time of billbox door opening)
        /// </summary>
        public const long EXC_SM_SVC_BB_STRT = 111;

        /// <summary>
        /// Billbox full
        /// </summary>
        public const long EXC_SM_BB_FULL = 167;

        /// <summary>
        /// Bill acceptor jammed
        /// </summary>
        public const long EXC_SM_BV_JAM = 177;

        /// <summary>
        /// Bill validator out of order (Cause N)
        /// </summary>
        public const long EXC_SM_BV_FAULT = 168;

        /// <summary>
        /// Low-level communication error with SM
        /// Tags:
        /// JD_DATA1 (reason)
        /// </summary>
        public const long EXC_SMIF_LOWLEVEL_COM_ERR = 1010;

        /// <summary>
        /// Gaming machine tilt
        /// </summary>
        public const long EXC_SM_TILT = 159;

        /// <summary>
        /// Service card in
        /// Tags:
        /// SC_CARDNR (Current service card number)
        /// </summary>
        public const long EXC_SVC_CARDIN = 5000;

        /// <summary>
        /// Service card out
        /// Tags:
        /// SC_CARDNR (Current service card number)
        /// </summary>
        public const long EXC_SVC_CARDOUT = 5001;

        /// <summary>
        /// Printer paper low
        /// </summary>
        public const long EXC_TICKET_PRINTER_PAPER_LOW = 4808;

        /// <summary>
        /// Printer paper out error
        /// </summary>
        public const long EXC_TICKET_PRINTER_PAPER_OUT = 4807;

        /// <summary>
        /// Printer power off
        /// </summary>
        public const long EXC_TICKET_PRINTER_PWR_OFF = 4809;

        /// <summary>
        /// Printer carriage jammed
        /// </summary>
        public const long EXC_TICKET_PRINTER_JAM = 4812;

        /// <summary>
        /// IGT/Cyberdyne ticket printer reports a problem
        /// Tags:
        /// PRINTER_ERRCODE (Error code of IGT/Cyberdyne ticket printers that is sent with exception
        /// 4802: 1 ... Printer disconnected,
        /// 2 ... Printer malfunction,
        /// 3 ... Printer communiction error,
        /// 4 ... Printer paper out,
        /// 5 ... Printer paper low,
        /// 6 ... Printer power on,
        /// 7 ... Printer power off,
        /// 8 ... Replace printer ribbon
        ///  9 ... Printer carriage jammed)
        /// </summary>
        public const long EXC_TICKET_PRINTER_ERR = 4802;

        /// <summary>
        /// MDC shutdown. If JD_DATA1 contains 1, the shutdown reason is powerfail; otherwise the shutdown is intentional.
        /// </summary>
        public const long EXC_MDC_SHUTDOWN = 9800;

        /// <summary>
        /// Denomination mismatch, DENOM shows denomination retrieved from gaming machine.
        /// Tags:
        /// JD_DATA1 (SM-Denom)
        /// JD_DATA2 (MDC-Denom / Config-Denom ??)
        /// </summary>
        public const long EXC_DENOMWRONG = 1001;

        public const long EXC_SM_RAM_CLEAR = 1399;

        /// <summary>
        /// Bill rejected
        /// </summary>
        public const long EXC_SM_BILL_REJ = 144;

        /// <summary>
        /// More than 5 consecutive bills rejected
        /// </summary>
        public const long EXC_SM_BILL_REJX5 = 145;

        /// <summary>
        /// Bill clearance done
        /// </summary>
        public const long EXC_SM_SCRL_DONE = 188;

        /// <summary>
        /// Bill clearance ended by system command
        /// </summary>
        public const long EXC_SM_SCLR_END = 1201;

        /// <summary>
        /// Sent when a player card is rejected
        /// </summary>
        public const long CARD_REJECT = 4021;

        /// <summary>
        /// Cancel a pending handpay (cancelled by player)
        /// </summary>
        public const long CC_CANCEL	= 1111;
        
        /// <summary>
        /// Handpay PIN authentication is finished.
        /// </summary>
        public const long HP_AUTH =	1113;

        /// <summary>
        /// Cashless Handpay is pending
        /// </summary>
        public const long CCHP = 4031;

        /// <summary>
        /// Cashless Handpay reset (handpay has been completed)
        /// </summary>
        public const long CCHP_RESET = 4032;

        /// <summary>
        /// Missing Card Handpay is pending.
        /// </summary>
        public const long MCHP = 4033;

        /// <summary>
        /// Missing Card Handpay has been completed.
        /// </summary>
        public const long MCHP_PAID = 4034;

        /// <summary>
        /// Missing Card Handpay has been cancelled.
        /// </summary>
        public const long MCHP_CANC = 4035;

        /// <summary>
        /// Gaming machine detected a ROM error.
        /// </summary>
        public const long SM_ROM_ERROR = 162;

        /// <summary>
        /// Gaming machine detected an error in the non-volatile RAM (Cause N).
        /// </summary>
        public const long SM_NVRAM_ERROR = 163;

        /// <summary>
        /// Operator changed options on gaming machine.
        /// </summary>
        public const long SM_OPTION_CHG = 165;

        /// <summary>
        /// Mystery jackpot processing has been completed.
        /// </summary>
        public const long MYST_JP_RESET = 1095;

        /// <summary>
        /// Jackpot payout to player account failed.
        /// </summary>
        public const long PTC_REJECTED = 1096;

        /// <summary>
        /// Cashless purses cleared by attendant.
        /// </summary>
        public const long CL_ERR_CL_COM = 4005;

        /// <summary>
        /// Service code entered
        /// </summary>
        public const long SVC_CODE = 5002;

        /// <summary>
        /// Result of gaming machine EPROM check. SM_EPROMID is sent for every EPROM (up to 16).
        /// </summary>
        public const long SM_EPROM_RES = 9101;

        /// <summary>
        /// Local jackpot hit value. In case of a Mystery hit, there may be one exception for every gaming machine
        /// </summary>
        public const long LOC_JCK_HIT_VAL =	1103;
    }
}
