#region

using System;
using System.Collections.Generic;
using FloorServer.Client.Commands;
using FloorServer.Client.Configuration;
using FloorServer.Client.Ect;
using FloorServer.Client.Enums;
using FloorServer.Client.Tools;
using FloorServer.Client.Boss;
using FloorServer.Client.Communication;

#endregion

namespace FloorServer.Client
{
    public interface IFloorClient : IDisposable
    {
        #region events

        event EventHandler<FloorEventArgs> ExceptionReceived;
        event EventHandler<FloorQueryArgs> QueryResponseReceived;
        event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChanged;
        event EventHandler<EctEventArgs> EctResponseReceived;

        #endregion

        #region Properties

        BossCommandBuilder CommandBuilder { get; }

        bool AckEnabled
        { get; set; }

        ConnectionStatus Status
        { get; }

        FloorServerConfiguration Config { get; }

        #endregion

        /// <summary>
        /// Connects to the client to the Floor.
        /// The connection process causes the ConnectionStatusChanged event to be raised (to Connecting and Connected).                
        /// </summary>
        bool Connect();

        /// <summary>
        /// Disconnects from the Floor.
        /// Once disconnected, client will not receive exception anymore.
        /// The disconnection process causes the ConnectionStatusChanged event to be raised (to Disconnected).                
        /// </summary>
        void Close();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="table"></param>
        /// <param name="whereClause"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        IQueryReader Query(string table, string whereClause, params string[] fields);

        /// <summary>
        /// Send the given query in asynchrone
        /// </summary>
        /// <param name="table"></param>
        /// <param name="whereClause"></param>
        /// <param name="fields"></param>
        /// <returns></returns>
        void QueryAsync(string table, string whereClause, params string[] fields);

        /// <summary>
        /// Registers the client to the specified exception.
        /// </summary>
        /// <param name="exception">The exception to register for.</param>
        /// <param name="requiredFields">The required fields appended to the floor message when the exception is received.</param>
        void Register(long exception, params string[] requiredFields);

        /// <summary>
        /// Registers the client to the specified exception.
        /// </summary>
        /// <param name="exception">The exception to register for.</param>
        /// <param name="persisted">if set to <c>true</c> [persisted].</param>
        /// <param name="requiredFields">The required fields appended to the floor message when the exception is received.</param>
        void Register(long exception, bool persisted, params string[] requiredFields);

        /// <summary>
        /// Registers the client to the specified exception.
        /// </summary>
        /// <param name="exception">The exception to register for.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="persisted">if set to <c>true</c> the server will keep the exception message in its persistent storage until it has been sucessfully delivered to the client.</param>
        /// <param name="requiredFields">The required fields appended to the floor message when the exception is received.</param>
        void Register(long exception, string whereClause, bool persisted, params string[] requiredFields);

        /// <summary>
        /// Registers the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="persisted">if set to <c>true</c> [persisted].</param>
        /// <param name="clientName">The registering client name. Can be different from the current one</param>
        /// <param name="requiredFields">The fields required from the exception</param>
        void Register(long exception, string whereClause, bool persisted, string clientName,
                             params string[] requiredFields);

        /// <summary>
        /// The client will not receive the specified exception from the Floor anymore.
        /// </summary>
        /// <param name="exception">The exception to unregister for.</param>
        void Unregister(long exception);

        /// <summary>
        /// Unregisters the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="clientName">The unregistering client name. Can be different from the current on</param>
        void Unregister(long exception, string clientName);

        /// <summary>
        /// The client will not receive exceptions from the Floor anymore.
        /// </summary>
        void UnregisterAll();

        /// <summary>
        /// Clear all periodic readings for a readName.
        /// </summary>
        /// <param name="readName">Destination (client) for which clearing the readings.</param>
        void ClearPeriodicReading(string readName);

        /// <summary>
        /// Builds the periodic reading command.
        /// </summary>
        /// <param name="readName">Destination (client) where to send the read results.</param>
        /// <param name="readID">ID of reading.</param>
        /// <param name="readedFields">The readed fields.</param>
        /// <param name="readRepeat">The read repeat.</param>
        /// <param name="readOffset">Start of reading. Incompatible with readTime.</param>
        /// <param name="whereClause">Where clause to select a set of machines for this reading.</param>
        /// <param name="sendTerminatorException">if set to <c>true</c> the FS will an exception 2003 at the end of the periodic reading.</param>
        /// <param name="persisted">if set to <c>true</c> [persisted].</param>
        void CreatePeriodicReading(PeriodicReadingCommand periodicReadingCommand);

        void CreatePeriodicReadings(List<PeriodicReadingCommand> periodicReadingCommands, bool persistCommand);

        void ClearPersistedCommands();

        #region Live Messaging

        /// <summary>
        /// Sends a live message to the Floor.
        /// </summary>
        /// <param name="flags">Flags to specified how the live message will be processed.</param>
        /// <param name="messageID">The live message ID to be sent.</param>
        /// <param name="whereClause">Where clause to select a set of slot machines.</param>
        /// <seealso cref="LiveMessagingFlags"/>
        /// /// <seealso cref="FloorCommandHelper"/>
        void SendLiveMessage(LiveMessagingFlags flags, ushort messageID, string whereClause, string cmodLanguageCode);

        /// <summary>
        /// Sends a live message to the Floor.
        /// </summary>
        /// <param name="flags">Flags to specified how the live message will be processed.</param>
        /// <param name="messageID">The live message ID to be sent.</param>
        /// <param name="whereClause">Where clause to select a set of slot machines.</param>
        /// <param name="text">Text to display on the selected slot machines.</param>
        /// <seealso cref="LiveMessagingFlags"/>
        /// <seealso cref="FloorCommandHelper"/>
        void SendLiveMessage(LiveMessagingFlags flags, ushort messageID, string whereClause, string text, string cmodLanguageCode);

        /// <summary>
        /// Sends a live message to the Floor.
        /// </summary>
        /// <param name="flags">Flags to specified how the live message will be processed.</param>
        /// <param name="messageID">The live message ID to be sent.</param>
        /// <param name="whereClause">Where clause to select a set of slot machines.</param>
        /// <param name="text">Text to display on the selected slot machines.</param>
        /// <param name="duration">The duration in seconds you want the message is displayed (exact duration is not garantee).</param>
        /// <seealso cref="LiveMessagingFlags"/>
        /// <seealso cref="FloorCommandHelper"/>
        void SendLiveMessage(LiveMessagingFlags flags, ushort messageID, string whereClause, string text, string cmodLanguageCode, int duration);

        /// <summary>
        /// Sends a live message to the Floor.
        /// </summary>
        /// <param name="flags">Flags to specified how the live message will be processed.</param>
        /// <param name="messageID">The live message ID to be sent.</param>
        /// <param name="whereClause">Where clause to select a set of slot machines.</param>
        /// <param name="text">Text to display on the selected slot machines.</param>
        /// <param name="duration">The duration in seconds you want the message is displayed (exact duration is not garantee).</param>
        /// <param name="cardNumber">Card number for live message display control.</param>
        /// <seealso cref="LiveMessagingFlags"/>
        /// <seealso cref="FloorCommandHelper"/>
        void SendLiveMessage(LiveMessagingFlags flags, ushort messageID, string whereClause, string text, string cmodLanguageCode, int duration, string cardNumber);

        /// <summary>
        /// Sends a live message to the Floor.
        /// </summary>
        /// <param name="flags">Flags to specified how the live message will be processed.</param>
        /// <param name="messageID">The live message ID to be sent.</param>
        /// <param name="whereClause">Where clause to select a set of slot machines.</param>
        /// <param name="text">Text to display on the selected slot machines.</param>
        /// <param name="duration">The duration in seconds you want the message is displayed (exact duration is not garantee).</param>
        /// <param name="cardNumber">Card number for live message display control.</param>
        /// <param name="hostMessageID">32-bit host ID generated by the system. Floor uses this ID in the live message result exception</param>
        /// <seealso cref="LiveMessagingFlags"/>
        /// <seealso cref="FloorCommandHelper"/>
        void SendLiveMessage(LiveMessagingFlags flags, ushort messageID, string whereClause, string text, string cmodLanguageCode, int duration, string cardNumber, ulong hostMessageID);

        void SendLiveMessage(LiveMessagingFlags flags, string priority, ushort messageId, string buttonText,
                string button2Text, string text, string cmodLanguageCode, int duration, string resources, string templateId,
                string withAnswer, string cardNumber, ulong hostMessageId, string whereClause);


        #endregion

        #region Ticket Management

        /// <summary>
        /// Sends the ticket redemption command according to the parameters sent by the Ticket System.
        /// </summary>
        /// <param name="validationNumber">The validation number.</param>
        /// <param name="voucherTransferCode">The voucher transfer code.</param>
        /// <param name="amountCents">The amount cents.</param>
        /// <param name="restrictedExpiration">The restricted expiration.</param>
        /// <param name="restrictedPoolID">The restricted pool ID.</param>
        /// <param name="whereClause">The where clause.</param>
        void SendTicketRedemptionCommand(string validationNumber, short voucherTransferCode, long amountCents, long restrictedExpiration, int restrictedPoolID, string whereClause);

        /// <summary>
        /// Sends the ticket issue command.
        /// </summary>
        /// <param name="validationNumber">The validation number.</param>
        /// <param name="cashoutType">Type of the cashout.</param>
        /// <param name="whereClause">The where clause.</param>
        void SendTicketIssueCommand(string validationNumber, short validationSystemID, string whereClause);

        /// <summary>
        /// Sends the ticket config update command.
        /// </summary>
        /// <param name="shortName">The short name.</param>
        /// <param name="addressLine1">The address line1.</param>
        /// <param name="addressLine2">The address line2.</param>
        /// <param name="restrictedText">The restricted text.</param>
        /// <param name="debitText">The debit text.</param>
        /// <param name="expiresMachineAfter">The expires machine after.</param>
        /// <param name="expiresPromotionalAfter">The expires promotional after.</param>
        /// <param name="controlFlags">The control flags.</param>
        /// <param name="whereClause">The where clause.</param>
        void SendTicketConfigUpdateCommand(string shortName, string addressLine1, string addressLine2, string restrictedText, string debitText, int expiresMachineAfter, int expiresPromotionalAfter, ValidationControlFlags controlFlags, string whereClause);

        /// <summary>
        /// Sends the ticket validation parameters.
        /// </summary>
        /// <param name="validationID">The validation ID.</param>
        /// <param name="validationSequenceNbr">The validation sequence NBR.</param>
        /// <param name="whereClause">The where clause.</param>
        void SendTicketValidationParameters(long validationID, long validationSequenceNbr, string whereClause);

        /// <summary>
        /// Sends the ticket system status command.
        /// </summary>
        /// <param name="isConnected">if set to <c>true</c> [is connected].</param>
        void SendTicketSystemStatusCommand(bool isConnected);

        #endregion

        #region Points Management

        void NotifyPlayerPointsUpdated(string cardNumber, double pointsBalance, string whereClause);

        #endregion

        #region ECT management

        IEctTransaction SendEctStartCommand(EctDestination destination, string transferId, long amount, EctCreditType creditType, string playerCardNumber, string egmId, string message, long timeout, int responseTimeout);

        #endregion

        #region Happy Hour Management

        /// <summary>
        /// Sends a command to create a group of SM for an happy hour
        /// </summary>
        /// <param name="hhGroupId">The group id</param>
        /// <param name="hhGroupName">The group name</param>
        /// <param name="bonus">The bonus to apply during the happy hour</param>
        /// <param name="resourceId">The id of the person responsible for the creation of this group</param>
        /// <param name="startDate">The start date of the happy hour</param>
        /// <param name="endDate">The end date of the happy hour</param>
        void SendCreateHappyHourGroupCommand(string hhGroupId, string hhGroupName, double bonus, string resourceId, DateTime startDate, DateTime endDate);

        /// <summary>
        /// Sends a command to add a slot machine to an happy hour group
        /// </summary>
        /// <param name="hhGroupId">The id of the group</param>
        /// <param name="smHwId">The hardware id of the SM</param>
        void SendAddSMToGroupCommand(string hhGroupId, string smHwId);

        /// <summary>
        /// Sends a command to remove an happy hour group
        /// </summary>
        /// <param name="hhGroupId">The id of the group</param>
        void SendRemoveHappyHourGroupCommand(string hhGroupId);

        /// <summary>
        /// Sends a command to remove a SM from an happy hour group
        /// </summary>
        /// <param name="hhGroupId">The id of the group</param>
        /// <param name="smHwId">The hardware id of the SM</param>
        void SendRemoveSMFromHappyHourGroupCommand(string hhGroupId, string smHwId);

        #endregion

        void Send(Bom message);

        void LockEGMs(string whereClause);

        void UnlockEGMs(string whereClause);

        void Reset(string whereClause);

        void DestroyConfig(string whereClause);

        void CashoutEGMs(string whereClause, bool lockBeforeCashout);

        void EnableMoneyIn(string whereClause);

        void DisableMoneyIn(string whereClause);

        #region WelcomeCredits

        void SendWelcomeCreditsEligibilityCommand(
            string smHwId,
            string cardInSeqId,
            decimal cashAmount,
            decimal promoAmount,
            string promotionName,
            string playerLanguageCode);

        void SendWelcomeCreditsOfferResponseCommand(string smHwId, string cardInSeqId, int resultCode, decimal? cashBalance, decimal? promoBalance);

        #endregion
    }
}
