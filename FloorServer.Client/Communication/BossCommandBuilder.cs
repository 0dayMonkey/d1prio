#region

using System;
using System.Collections.Generic;
using System.Globalization;
using FloorServer.Client.Boss;
using FloorServer.Client.Communication.Details;
using FloorServer.Client.Ect;
using FloorServer.Client.Enums;
using FloorServer.Client.Tools;
using FloorServer.Client.Tools.Validators;
using Microsoft.Extensions.Logging;

#endregion

namespace FloorServer.Client.Communication
{
    public class BossCommandBuilder
    {
        private readonly string _clientName;
        private readonly BomFactory _bomFactory;
        private readonly ILoggerFactory _loggerFactory;
        private readonly ILogger<BossCommandBuilder> _logger;

        public BossCommandBuilder(string clientName, BomFactory bomFactory, ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
            _logger = _loggerFactory.CreateLogger<BossCommandBuilder>();
            _clientName = clientName;
            _bomFactory = bomFactory;
        }

        /// <summary>
        /// Appends the sequence number.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="sequenceNumber">The sequence number.</param>
        /// <returns></returns>
        public Bom AppendSequenceNumber(Bom msg, int sequenceNumber)
        {
            if (msg == null)
                throw new ArgumentNullException("msg");
            msg.PutString(Keys.SEQ_NBR, sequenceNumber.ToString());

            return msg;
        }

        /// <summary>
        /// Builds the command related to protocol.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        private Bom BuildProtocolCommand(string command, CommandParameters parameters)
        {
            switch (command)
            {
                case CommandKeys.ACK:
                    var src = parameters[Tags.SRC] as Bom;
                    var msg = _bomFactory.CreateBom(src);
                    msg.PutString(Tags.DST, src.GetString(Tags.SRC));
                    msg.PutString(Tags.SRC, src.GetString(Tags.DST));
                    return msg;
                case CommandKeys.RENAME:
                    var versionInfo = new BOMVersionInfo(parameters[Tags.NAME].ToString(), _loggerFactory);
                    return versionInfo;
            }
            return null;
        }

        /// <summary>
        /// Builds the command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public Bom BuildCommandBase(string command)
        {
            //set commands common tags 
            var cmdBase = _bomFactory.CreateBom();
            cmdBase.PutString(Tags.ACT, Tags.CMD);
            cmdBase.PutString(Tags.CMD, command);
            cmdBase.PutString(Tags.SRC, _clientName);
            cmdBase.PutString(Tags.DST, Tags.FS);

            return cmdBase;
        }

        private Bom BuildQueryBase()
        {
            var queryBase = _bomFactory.CreateBom();
            queryBase.PutString(Tags.ACT, Tags.QUERY);
            queryBase.PutString(Tags.SRC, _clientName);
            queryBase.PutString(Tags.DST, Tags.FS);

            return queryBase;
        }

        /// <summary>
        /// Builds the live message command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public Bom BuildLiveMessageCommand(
            LiveMessagingFlags flags,
            ushort messageID,
            string whereClause,
            string text,
            string cmodLanguageCode,
            int duration,
            string cardNumber,
            ulong hostMessageID)
        {
            var values = _bomFactory.CreateBom();
            values.PutString(ProtocolKeys.LIVE_MSG_FLAGS, ((ushort) flags).ToString());
            values.PutString(ProtocolKeys.LIVE_MSGID, messageID.ToString());

            if (hostMessageID > 0)
                values.PutString(ProtocolKeys.LIVE_MSG_HOSTID, hostMessageID.ToString());

            if (string.IsNullOrEmpty(text) || new LiveMessageTextValidator().Validate(text))
                values.PutString(ProtocolKeys.LIVE_TEXT, string.IsNullOrEmpty(text) ? string.Empty : text);

            if (!string.IsNullOrEmpty(cardNumber))
                values.PutString(ProtocolKeys.LIVE_MSG_CARDNR, cardNumber);

            if (!string.IsNullOrEmpty(cmodLanguageCode))
                values.PutString(ProtocolKeys.LIVE_MSG_LANGUAGE_CODE, cmodLanguageCode);

            if (duration > 0)
            {
                if ((flags & LiveMessagingFlags.LiveMessageExpiry) == LiveMessagingFlags.LiveMessageExpiry)
                {
                    values.PutString(ProtocolKeys.LIVE_MSG_EXPIRY,
                                     DateTools.DateTimeToUnixTime(DateTime.Now.AddSeconds(duration)).ToString());
                }
                else
                    _logger.LogInformation(
                        "{0} FLAG missing, expiry timestamp will be ignored",
                        LiveMessagingFlags.LiveMessageExpiry.ToString());
            }

            //create the message according to values
            Bom msg = BuildCommandBase(CommandKeys.CMD_LIVE_MSG);
            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Tags.WHERE, whereClause);
            msg.PutMsg(Tags.VALUES, values);

            return msg;
        }

        public Bom BuildLiveMessageCommand(
            LiveMessagingFlags flags,
            string priority,
            ushort messageID,
            string buttonText,
            string button2Text,
            string text,
            string cmodLanguageCode,
            int duration,
            string resources,
            string templateId,
            string withAnswer,
            string cardNumber,
            ulong hostMessageID,
            string whereClause)
        {
            var values = _bomFactory.CreateBom();
            values.PutString(ProtocolKeys.LIVE_MSG_FLAGS, ((ushort) flags).ToString());
            values.PutString(ProtocolKeys.LIVE_MSGID, messageID.ToString());

            if (hostMessageID > 0)
                values.PutString(ProtocolKeys.LIVE_MSG_HOSTID, hostMessageID.ToString());

            if (string.IsNullOrEmpty(text) || new LiveMessageTextValidator().Validate(text))
                values.PutString(ProtocolKeys.LIVE_TEXT, string.IsNullOrEmpty(text) ? string.Empty : text);

            if (!string.IsNullOrEmpty(cardNumber))
                values.PutString(ProtocolKeys.LIVE_MSG_CARDNR, cardNumber);

            if (duration > 0)
            {
                values.PutString(ProtocolKeys.LIVE_MSG_DURATION, "" + duration);
            }

            if (!string.IsNullOrEmpty(cmodLanguageCode))
                values.PutString(ProtocolKeys.LIVE_MSG_LANGUAGE_CODE, cmodLanguageCode);

            if (!string.IsNullOrEmpty(priority))
                values.PutString(ProtocolKeys.LIVE_MSG_PRIORITY, priority);

            if (!string.IsNullOrEmpty(buttonText))
                values.PutString(ProtocolKeys.LIVE_MSG_BUTTON_TEXT, buttonText);

            if (!string.IsNullOrEmpty(button2Text))
                values.PutString(ProtocolKeys.LIVE_MSG_BUTTON2_TEXT, button2Text);

            if (!string.IsNullOrEmpty(resources))
                values.PutString(ProtocolKeys.LIVE_MSG_RESOURCES, resources);

            if (!string.IsNullOrEmpty(templateId))
                values.PutString(ProtocolKeys.LIVE_MSG_TEMPLATE_ID, templateId);

            if (!string.IsNullOrEmpty(withAnswer))
                values.PutString(ProtocolKeys.LIVE_MSG_WITH_ANSWER, withAnswer);

            //create the message according to values
            Bom msg = BuildCommandBase(CommandKeys.CMD_LIVE_MSG);
            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Tags.WHERE, whereClause);
            msg.PutMsg(Tags.VALUES, values);

            return msg;
        }

        /// <summary>
        /// Builds the rename command.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        public Bom BuildRenameCommand(string newName)
        {
            var parameters = new CommandParameters();
            parameters.Add(Tags.NAME, newName);

            return BuildProtocolCommand(CommandKeys.RENAME, parameters);
        }

        public Bom BuildHelloCommand(int? winsize)
        {
            Bom msg = BuildCommandBase(CommandKeys.CMD_HELLO);

            if (winsize.HasValue) msg.PutString("WINSIZE", winsize.Value.ToString());

            return msg;
        }

        /// <summary>
        /// Builds the acknowledge.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns></returns>
        public Bom BuildAcknowledge(Bom message)
        {
            var parameters = new CommandParameters();
            parameters.Add(Tags.SRC, message);

            return BuildProtocolCommand(CommandKeys.ACK, parameters);
        }

        /// <summary>
        /// Builds the register command.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="persisted">if set to <c>true</c> [persisted].</param>
        /// <param name="clientName">The registering client name. Can be different from the current one</param>
        /// <param name="requiredFields">The fields required from the exception</param>
        /// <returns></returns>
        public Bom BuildRegisterCommand(long exception, string whereClause, bool persisted, string clientName, params string[] requiredFields)
        {
            var values = _bomFactory.CreateBom();
            values.PutString(Tags.ACK_REQU, persisted ? "1" : "0");
            values.PutString(Tags.MSG_REQU, exception.ToString());
            values.PutString(Tags.NAME, clientName);
            values.PutString(Tags.WHERE_REQU, string.IsNullOrEmpty(whereClause) ? "1" : whereClause);
            if (requiredFields != null)
                values.PutStrList(Tags.FIELDS_REQU, new List<string>(requiredFields));

            Bom msg = BuildCommandBase(CommandKeys.CMD_REGISTER);
            msg.PutMsg(Tags.VALUES, values);
            return msg;
        }

        /// <summary>
        /// Builds the unregister command.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="clientName">The unregistering client name. Can be different from the current one</param>
        /// <returns></returns>
        public Bom BuildUnregisterCommand(long exception, string clientName)
        {
            Bom msg = BuildCommandBase(CommandKeys.CMD_UNREGISTER);
            msg.PutString(Tags.MSG_REQU, exception.ToString());
            msg.PutString(Tags.WHERE, FloorCommandHelper.Equal(Fields.NAME, clientName));
            return msg;
        }


        /// <summary>
        /// Builds the periodic reading command.
        /// </summary>
        /// <param name="readName">Destination (client) where to send the read results.</param>
        /// <param name="readID">ID of reading.</param>
        /// <param name="readedFields">The readed fields.</param>
        /// <param name="readRepeat">The read repeat.</param>
        /// <param name="readTime">Time of reading. Incompatible with readOffset.</param>
        /// <param name="readOffset">Start of reading. Incompatible with readTime.</param>
        /// <param name="whereClause">Where clause to select a set of machines for this reading.</param>
        /// <param name="sendTerminatorException">if set to <c>true</c> the FS will an exception 2003 at the end of the periodic reading.</param>
        /// <param name="persisted">if set to <c>true</c> [persisted].</param>
        /// <returns>Bom message</returns>
        ///         
        public Bom BuildPeriodicReadingCommand(
            string readName,
            ulong readID,
            ulong readRepeat,
            TimeSpan? readOffset,
            ulong? readTime,
            string whereClause,
            bool persisted,
            bool sendTerminatorException,
            params string[] readedFields)
        {
            var values = _bomFactory.CreateBom();
            values.PutString(Tags.ACK_REQU, persisted ? "1" : "0");
            values.PutString(Keys.TERMINATOR, sendTerminatorException ? "1" : "0");
            values.PutString(Tags.READ_ID, readID.ToString());
            values.PutString(Tags.READ_NAME, string.IsNullOrWhiteSpace(readName) ? _clientName : readName);
            values.PutString(Tags.READ_TIME, readTime == null ? "0" : readTime.ToString());
            if (readOffset != null)
                values.PutString(Keys.READ_OFFSET,
                                 string.Format("{0}:{1}", readOffset.Value.Hours, readOffset.Value.Minutes));
            values.PutString(Tags.READ_REPEAT, readRepeat.ToString());

            if (!string.IsNullOrEmpty(whereClause))
                values.PutString(Tags.READ_WHERE, whereClause);

            if (readedFields != null)
                values.PutStrList(Tags.READ_FIELDS, new List<string>(readedFields));

            Bom msg = BuildCommandBase(CommandKeys.CMD_ADD_READING);
            msg.PutMsg(Tags.VALUES, values);
            return msg;
        }

        /// <summary>
        /// Clear all periodic readings for a readName.
        /// </summary>
        /// <param name="readName">Destination (client) for which clearing the readings.</param>
        /// <returns>Bom message</returns>
        public Bom BuildClearReadingCommand(string readName)
        {
            if (string.IsNullOrWhiteSpace(readName))
                throw new ArgumentNullException("readName");

            Bom msg = BuildCommandBase(CommandKeys.CMD_DEL_READING);
            msg.PutString(Tags.WHERE, FloorCommandHelper.Equal(Fields.READ_NAME, readName));
            return msg;
        }

        public Bom BuildLockCommand(string whereClause)
        {
            Bom msg = BuildCommandBase(CommandKeys.LOCK_SM);
            msg.PutString(Tags.WHERE, whereClause);
            return msg;
        }

        public Bom BuildUnlockCommand(string whereClause)
        {
            Bom msg = BuildCommandBase(CommandKeys.UNLOCK_SM);
            msg.PutString(Tags.WHERE, whereClause);
            return msg;
        }

        public Bom BuildResetCommand(string whereClause)
        {
            Bom msg = BuildCommandBase(CommandKeys.RESET_MDC);
            msg.PutString(Tags.WHERE, whereClause);
            return msg;
        }

        public Bom BuildDestroyConfigCommand(string whereClause)
        {
            Bom msg = BuildCommandBase(CommandKeys.DESTROY_CONFIG);
            msg.PutString(Tags.WHERE, whereClause);
            return msg;
        }

        public Bom BuildEnableMoneyInCommand(string whereClause)
        {
            Bom msg = BuildCommandBase(CommandKeys.CMD_CASHIN_ENABLE);
            msg.PutString(Tags.WHERE, whereClause);
            return msg;
        }

        public Bom BuildDisableMoneyInCommand(string whereClause)
        {
            Bom msg = BuildCommandBase(CommandKeys.CMD_CASHIN_DISABLE);
            msg.PutString(Tags.WHERE, whereClause);
            return msg;
        }

        public Bom BuildCashoutCommand(string whereClause, bool lockBeforeCashout)
        {
            var values = _bomFactory.CreateBom();
            values.PutString(Keys.CASHOUT_FLAGS, lockBeforeCashout ? "1" : "0");

            Bom msg = BuildCommandBase(CommandKeys.CASHOUT_SM);
            msg.PutString(Tags.WHERE, whereClause);
            msg.PutMsg(Tags.VALUES, values);
            return msg;
        }

        public Bom BuildPeristedCommandBody()
        {
            return BuildCommandBase(CommandKeys.CMD_PERSIST);
        }

        public Bom ClearPersistedCommands()
        {
            return BuildCommandBase(CommandKeys.CMD_DEL_PERSIST);
        }

        #region TITO Commands

        /// <summary>
        /// Builds the ticket system status command.
        /// </summary>
        /// <param name="isConnected">if set to <c>true</c> [is connected].</param>
        /// <returns></returns>
        public Bom BuildTicketSystemStatusCommand(bool isConnected)
        {
            Bom msg = BuildCommandBase(CommandKeys.CMD_TICKET_LINK_UPDATE);
            msg.PutString(Keys.TITO_LINK_STATUS, isConnected ? "UP" : "DOWN");

            return msg;
        }

        /// <summary>
        /// Builds the ticket redemption command.
        /// </summary>
        /// <param name="validationNumber">The validation number.</param>
        /// <param name="voucherTransferCode">The voucher transfer code.</param>
        /// <param name="transferAmount">The transfer amount.</param>
        /// <param name="restrictedExpiration">The restricted expiration.</param>
        /// <param name="restrictedPoolID">The restricted pool ID.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns></returns>
        public Bom BuildTicketRedemptionCommand(string validationNumber, short voucherTransferCode, long transferAmount,
                                                long restrictedExpiration, int restrictedPoolID, string whereClause)
        {
            if (string.IsNullOrEmpty(validationNumber))
                return null;

            var values = _bomFactory.CreateBom();
            values.PutString(Keys.TITO_VAL_DATA, validationNumber);
            values.PutString(Keys.TITO_REDEMPTION_REPLY, voucherTransferCode.ToString());
            values.PutLong(Keys.TITO_AMOUNT, transferAmount);
            values.PutString(Keys.TITO_REDEMPTION_EXP, restrictedExpiration.ToString());
            values.PutString(Keys.TITO_POOL_ID, restrictedPoolID.ToString());

            Bom msg = BuildCommandBase(CommandKeys.CMD_TICKET_REDEMPTION);
            msg.PutMsg(Tags.VALUES, values);


            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Keys.WHERE, whereClause);

            return msg;
        }

        public Bom BuildTicketConfigUpdateCommand(string shortName, string addressLine1, string addressLine2,
                                                  string restrictedText, string debitText, int expiresMachineAfter,
                                                  int expiresPromotionalAfter, ValidationControlFlags controlFlags,
                                                  string whereClause)
        {
            var values = _bomFactory.CreateBom();

            values.PutString(Keys.TITO_CFG_LOC, string.IsNullOrEmpty(shortName) ? string.Empty : shortName);
            values.PutString(Keys.TITO_CFG_ADD1, string.IsNullOrEmpty(addressLine1) ? string.Empty : addressLine1);
            values.PutString(Keys.TITO_CFG_ADD2, string.IsNullOrEmpty(addressLine2) ? string.Empty : addressLine2);
            values.PutString(Keys.TITO_CFG_RESTRICT_TITLE,
                             string.IsNullOrEmpty(restrictedText) ? string.Empty : restrictedText);
            values.PutString(Keys.TITO_CFG_DEBIT_TITLE, string.IsNullOrEmpty(debitText) ? string.Empty : debitText);
            values.PutString(Keys.TITO_CFG_CASH_EXP, expiresMachineAfter.ToString());
            values.PutString(Keys.TITO_CFG_RESTRICT_EXP, expiresPromotionalAfter.ToString());
            values.PutString(Keys.TITO_CFG_CTRL, ((int) controlFlags).ToString());

            Bom msg = BuildCommandBase(CommandKeys.CMD_TICKET_CFG_UPDATE);
            msg.PutMsg(Tags.VALUES, values);

            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Keys.WHERE, whereClause);

            return msg;
        }

        /// <summary>
        /// Builds the ticket validation parameters command.
        /// </summary>
        /// <param name="validationID">The validation ID.</param>
        /// <param name="validationSeqNbr">The validation seq NBR.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns></returns>
        public Bom BuildTicketValidationParametersCommand(long validationID, long validationSeqNbr, string whereClause)
        {
            var values = _bomFactory.CreateBom();

            values.PutLong(Keys.TITO_VAL_CFG_MACH_ID, validationID);
            values.PutLong(Keys.TITO_VAL_CFG_SEQ_NR, validationSeqNbr);

            Bom msg = BuildCommandBase(CommandKeys.CMD_TICKET_VALIDATION_PARAMS);
            msg.PutMsg(Tags.VALUES, values);

            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Keys.WHERE, whereClause);

            return msg;
        }

        /// <summary>
        /// Builds the ticket issue command.
        /// </summary>
        /// <param name="validationNumber">The validation number.</param>
        /// <param name="validationSystemID">The validation system ID.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <returns></returns>
        public Bom BuildTicketIssueCommand(string validationNumber, short validationSystemID, string whereClause)
        {
            if (string.IsNullOrEmpty(validationNumber))
                return null;

            var values = _bomFactory.CreateBom();
            values.PutString(Keys.TITO_VAL_DATA, validationNumber);
            values.PutString(Keys.TITO_SYSTEM_ID, validationSystemID.ToString());

            Bom msg = BuildCommandBase(CommandKeys.CMD_TICKET_ISSUE);
            msg.PutMsg(Tags.VALUES, values);

            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Keys.WHERE, whereClause);

            return msg;
        }


        public Bom BuildPointsUpdateCommand(string cardNumber, double pointsBalance, string whereClause)
        {
            var values = _bomFactory.CreateBom();
            values.PutString(Keys.PL_CARD_NR, cardNumber.PadLeft(16, '0'));
            values.PutString(Keys.PL_POINTS, pointsBalance.ToString());
            values.PutString(Keys.PL_POINTS_TEXT, pointsBalance.ToString());

            Bom msg = BuildCommandBase(CommandKeys.CMD_POINTS_UPDATE);
            msg.PutMsg(Tags.VALUES, values);

            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Keys.WHERE, whereClause);

            return msg;
        }

        #endregion

        #region ECT Commands

        public Bom BuildEctStartCommand(EctDestination destination, string transferId, long amount,
                                        EctCreditType creditType, string playerCardNumber, string egmId, string message,
                                        long timeout)
        {
            var values = _bomFactory.CreateBom();
            values.PutString(Keys.ECT_CARD_NR, playerCardNumber);
            values.PutString(Keys.ECT_AMOUNT, amount.ToString());
            values.PutString(Keys.ECT_CREDIT_TYPE, "" + ((int) creditType));
            values.PutString(Keys.ECT_MESSAGE, message);
            values.PutString(Keys.ECT_FLAGS, ((short) EctFlags.NoUIAndForceECT).ToString());
            values.PutString(Keys.ECT_TIMEOUT, (timeout/1000).ToString());
            values.PutString(Keys.ECT_TRANSFER_ID, transferId);
            values.PutString(Keys.ECT_DESTINATION, "" + ((int) destination));
            Bom msg = BuildCommandBase(CommandKeys.CMD_ECT_START);
            msg.PutMsg(Tags.VALUES, values);
            if (!string.IsNullOrEmpty(egmId))
                msg.PutString(Keys.WHERE, FloorCommandHelper.Equal(Fields.SMDBID, egmId));
            return msg;
        }

        #endregion

        #region HappyHour Commands

        /// <summary>
        /// Create a group of Slot Machines for an happy hour
        /// </summary>
        /// <param name="happyHourGroupId">The group id</param>
        /// <param name="happyHourGroupName">The group name</param>
        /// <param name="bonus">The bonus applied during the happy hour</param>
        /// <param name="resourceId">The name of the host for the group</param>
        /// <param name="startDate">Start date of the happy hour</param>
        /// <param name="endDate">End date of the happy hour</param>
        /// <returns></returns>
        /// <summary>
        /// Create a group of Slot Machines for an happy hour
        /// </summary>
        /// <param name="happyHourGroupId">The group id</param>
        /// <param name="happyHourGroupName">The group name</param>
        /// <param name="bonus">The bonus applied during the happy hour</param>
        /// <param name="resourceId">The name of the host for the group</param>
        /// <param name="startDate">Start date of the happy hour</param>
        /// <param name="endDate">End date of the happy hour</param>
        /// <returns></returns>
        public Bom BuildCreateHappyHourGroupCommand(string happyHourGroupId, string happyHourGroupName, double bonus,
                                                    string resourceId, DateTime startDate, DateTime endDate)
        {
            if (string.IsNullOrEmpty(happyHourGroupId))
                return null;

            if (string.IsNullOrEmpty(happyHourGroupName))
                return null;

            if (string.IsNullOrEmpty(resourceId))
                return null;

            var values = _bomFactory.CreateBom();
            values.PutString(Keys.SM_GRP_BOSS, resourceId);
            values.PutString(Keys.SM_GRP_HOST, resourceId);
            values.PutString(Keys.SM_GRP_TYPE, "1");
            values.PutString(Keys.END_TIME, GetUniversalUnixTimestamp(endDate).ToString());
            values.PutString(Keys.SM_GRP_NAME, happyHourGroupName);
            values.PutString(Keys.SM_GRP_ID, happyHourGroupId);
            values.PutString(Keys.SM_PROM_MUL, bonus.ToString("0.##########", CultureInfo.CreateSpecificCulture("en-US")));
            values.PutString(Keys.START_TIME, GetUniversalUnixTimestamp(startDate).ToString());
            values.PutString(Keys.SM_POINTS, "0");

            Bom msg = BuildCommandBase(CommandKeys.CMD_CREATE_GROUP);
            msg.PutMsg(Tags.VALUES, values);

            return msg;
        }

        private long GetUniversalUnixTimestamp(DateTime date)
        {
            TimeSpan span = (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0));
            return (long) span.TotalSeconds;
        }

        /// <summary>
        /// Adds a slot machine to a group
        /// </summary>
        /// <param name="happyHourGroupId">The id of the group</param>
        /// <param name="smHwId">The slot machine hardware id</param>
        /// <returns></returns>
        public Bom BuildAddEGMToGroupCommand(string happyHourGroupId, string smHwId)
        {
            if (string.IsNullOrEmpty(happyHourGroupId))
                return null;

            if (string.IsNullOrEmpty(smHwId))
                return null;

            var values = _bomFactory.CreateBom();
            values.PutString(Keys.SMHWID, smHwId);
            values.PutString(Keys.SM_GRP_ID, happyHourGroupId);

            Bom msg = BuildCommandBase(CommandKeys.CMD_ADD_SM_TO_GROUP);
            msg.PutMsg(Tags.VALUES, values);

            return msg;
        }

        /// <summary>
        /// Remove an happy hour group
        /// </summary>
        /// <param name="happyHourGroupId">The id of the group to delete</param>
        /// <returns></returns>
        public Bom BuildDeleteHappyHourGroupCommand(string happyHourGroupId)
        {
            if (string.IsNullOrEmpty(happyHourGroupId))
                return null;

            Bom msg = BuildCommandBase(CommandKeys.CMD_DELETE_GROUP);

            string whereClause = FloorCommandHelper.Equal(Fields.SM_GRP_ID, happyHourGroupId);

            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Keys.WHERE, whereClause);

            return msg;
        }

        /// <summary>
        /// Remove a Slot Machine from a group
        /// </summary>
        /// <param name="happyHourGroupId">The id of the group</param>
        /// <param name="smHwId">The hardware id of the slot machine</param>
        /// <returns></returns>
        public Bom BuildDeleteEGMFromGroupCommand(string happyHourGroupId, string smHwId)
        {
            if (string.IsNullOrEmpty(happyHourGroupId))
                return null;

            Bom msg = BuildCommandBase(CommandKeys.CMD_REMOVE_SM_FROM_GROUP);

            string whereClause = FloorCommandHelper.And(FloorCommandHelper.Equal(Fields.SM_GRP_ID, happyHourGroupId),
                                                        FloorCommandHelper.Equal(Fields.SMHWID, smHwId));

            if (!string.IsNullOrEmpty(whereClause))
                msg.PutString(Keys.WHERE, whereClause);

            return msg;
        }

        #endregion

        #region Welcome Credits Commands

        public Bom BuildWelcomeCreditsEligibilityCommand(string smHwId, string cardInSeqId, decimal cashAmount, decimal promoAmount, string promotionName, string playerLanguageCode)
        {
            var command3050Message = _bomFactory.CreateBom();
            command3050Message.PutString(Keys.ACT, Keys.CMD);
            command3050Message.PutString(Keys.CMD, CommandKeys.CMD_WELCOME_CREDITS_ELIGIBILITY);
            command3050Message.PutString(Keys.DST, Keys.FS);

            command3050Message.PutString(Keys.SMHWID, smHwId);
            command3050Message.PutString(Keys.CISEQ, cardInSeqId);

            command3050Message.PutString(Keys.SRC, _clientName);
            command3050Message.PutString(Keys.WHERE, FloorCommandHelper.Equal(Fields.SMHWID, smHwId));

            var requestValues = _bomFactory.CreateBom();
            requestValues.PutString(Keys.WC_CASH_AMOUNT, cashAmount.ToString("0.####", CultureInfo.CreateSpecificCulture("en-US")));
            requestValues.PutString(Keys.WC_PROMO_AMOUNT, promoAmount.ToString("0.####", CultureInfo.CreateSpecificCulture("en-US")));
            requestValues.PutString(Keys.WC_OFFERED_AMOUNT, (promoAmount - cashAmount).ToString("0.####", CultureInfo.CreateSpecificCulture("en-US")));
            requestValues.PutString(Keys.WC_PROMO_NAME, promotionName);
            requestValues.PutString(Keys.PL_LANGUAGE, playerLanguageCode);

            command3050Message.PutMsg(Keys.VALUES, requestValues);

            return command3050Message;
        }

        public Bom BuildWelcomeCreditsOfferResponseCommand(string smHwId, string cardInSeqId, int resultCode, decimal? cashBalance, decimal? promoBalance)
        {
            var command3052Message = _bomFactory.CreateBom();
            command3052Message.PutString(Keys.ACT, Keys.CMD);
            command3052Message.PutString(Keys.CMD, CommandKeys.CMD_WELCOME_CREDITS_OFFER_RESPONSE);
            command3052Message.PutString(Keys.DST, Keys.FS);

            command3052Message.PutString(Keys.SMHWID, smHwId);
            command3052Message.PutString(Keys.CISEQ, cardInSeqId);

            command3052Message.PutString(Keys.SRC, _clientName);
            command3052Message.PutString(Keys.WHERE, FloorCommandHelper.Equal(Fields.SMHWID, smHwId));

            var requestValues = _bomFactory.CreateBom();
            requestValues.PutLong(Keys.WC_RESULT, resultCode);
            if (cashBalance != null && promoBalance != null)
            {
                requestValues.PutString(Keys.WC_CASH_CENTS_BALANCE, cashBalance.Value.ToString("0.####", CultureInfo.CreateSpecificCulture("en-US")));
                requestValues.PutString(Keys.WC_PROMO_CENTS_BALANCE, promoBalance.Value.ToString("0.####", CultureInfo.CreateSpecificCulture("en-US")));
            }

            command3052Message.PutMsg(Keys.VALUES, requestValues);

            return command3052Message;
        }

        #endregion

    }
}