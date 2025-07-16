using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Timers;
using FloorServer.Client.Boss;
using FloorServer.Client.Commands;
using FloorServer.Client.Communication;
using FloorServer.Client.Configuration;
using FloorServer.Client.Ect;
using FloorServer.Client.Enums;
using FloorServer.Client.Tools;
using Microsoft.Extensions.Logging;


namespace FloorServer.Client
{
    public class FloorServerClient : IFloorClient
    {

        public event EventHandler<ConnectionStatusEventArgs> ConnectionStatusChanged;
        public event EventHandler<FloorEventArgs> ExceptionReceived;
        public event EventHandler<FloorQueryArgs> QueryResponseReceived;

        private List<RegisteredException> _registeredExceptions = new List<RegisteredException>();
        private System.Timers.Timer _timer;
        private bool _runningTimer;


        /// <summary>
        /// Raises the <see cref="E:EventReceived"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FloorServer.Client.Tools.FloorEventArgs"/> instance containing the event data.</param>
        protected void OnExceptionReceived(FloorEventArgs e)
        {
            if (ExceptionReceived != null)
                ExceptionReceived(this, e);
        }

        protected void OnQueryResponseReceived(FloorQueryArgs e)
        {
            if (QueryResponseReceived != null)
                QueryResponseReceived(this, e);
        }

        /// <summary>
        /// Raises the <see cref="E:ConnectionStatusChanged"/> event.
        /// </summary>
        /// <param name="e">The <see cref="FloorServer.Client.Tools.ConnectionStatusEventArgs"/> instance containing the event data.</param>
        protected void OnConnectionStatusChanged(ConnectionStatus status)
        {
            _logger.LogDebug("Connection status changed to {0}", status.ToString());

            if (ConnectionStatusChanged != null)
                ConnectionStatusChanged(this, new ConnectionStatusEventArgs(status));
        }


        private readonly BossCommandBuilder _commandBuilder;
        private readonly FloorServerConfiguration _config;

        private readonly ILogger<FloorServerClient> _logger;
        private readonly ILoggerFactory _loggerFactory;

        private readonly Dictionary<string, EctResponseHandler> _ectHandlers =
            new Dictionary<string, EctResponseHandler>();

        private readonly Dictionary<string, QueryResponseHandler> _queryHandlers =
            new Dictionary<string, QueryResponseHandler>();

        private readonly Queue<Bom> _queuedMessages;
        private bool _ackEnabled;
        private BossClient _bossClient;
        private BossReceiver _bossReceiver;
        private BossTransmitter _bossTransmitter;
        private CancellationTokenSource _reconnectionCancellationTokenSource;
        private ConnectionStatus _status;

        private BomFactory _bomFactory;

        public BossCommandBuilder CommandBuilder { get { return _commandBuilder; } }


        /// <summary>
        /// Gets or sets the client.
        /// </summary>
        /// <value>The client.</value>
        public BossClient Client
        {
            get { return _bossClient; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [ack enabled].
        /// </summary>
        /// <value><c>true</c> if [ack enabled]; otherwise, <c>false</c>.</value>
        public bool AckEnabled
        {
            get { return _ackEnabled; }
            set
            {
                _ackEnabled = value;
                if (_bossTransmitter != null)
                    _bossTransmitter.WaitForAck = _ackEnabled;
            }
        }


        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public ConnectionStatus Status
        {
            get { return _status; }
            private set
            {
                if (_status != value)
                {
                    _status = value;
                    if (_status == ConnectionStatus.Connected)
                        SendQueuedMessages();

                    //raise connection status changed event
                    OnConnectionStatusChanged(_status);
                }
            }
        }

        public FloorServerConfiguration Config
        {
            get { return _config; }
        }

        public string ClientName { get; private set; }


        /// <summary>
        /// Initializes a new instance of the <see cref="FloorServerClient"/> class.
        /// </summary>
        /// <param name="config">The config.</param>
        public FloorServerClient(FloorServerConfiguration config, ILoggerFactory loggerfactory, string clientName = null)
        {
            ClientName = clientName ?? config.DefaultClientName;
            _loggerFactory = loggerfactory;
            _logger = _loggerFactory.CreateLogger<FloorServerClient>();
            if (config == null)
            {
                _logger.LogError(
                    "Unable to initialize FloorServerClient: configuration parameter cannot be null.");
                throw new ArgumentNullException();
            }

            _bomFactory = new BomFactory(loggerfactory);

            //if (!config.IsValid)
            //{
            //    _logger.LogError("Unable to initialize FloorServerClient: {0}",
            //                                                             config.Validator.ErrorMessage);
            //    throw new ArgumentException(config.Validator.ErrorMessage);
            //}
            //apply the valid configuration to the FloorServerClient
            _config = config;
            _commandBuilder = new BossCommandBuilder(ClientName, _bomFactory, _loggerFactory);
            _queuedMessages = new Queue<Bom>();

            InitTimer();
        }


        private void InitTimer()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
            }
            _timer = new System.Timers.Timer(30000);
            _timer.Elapsed += Timer_Elapsed;
            _timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_runningTimer) return;
            _runningTimer = true;
            try
            {
                if (Status != ConnectionStatus.Connected) return;
                CheckRegistration();
            }
            finally
            {
                _runningTimer = false;
            }
        }


        /// <summary>
        /// Connects to the client to the Floor.
        /// The connection process causes the ConnectionStatusChanged event to be raised (to Connecting and Connected).
        /// </summary>
        public bool Connect()
        {
            if (Status != ConnectionStatus.Disconnected)
                return true;

            return Connect(false);
        }

        /// <summary>
        /// Disconnects this instance.
        /// </summary>
        public virtual void Close()
        {
            try
            {
                if (_bossClient != null && Status == ConnectionStatus.Connected)
                {
                    _status = ConnectionStatus.Closed;
                    _bossClient.Stop();
                }
                _logger.LogInformation("Connection to FloorServer closed.");
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "An error occured while closing connection. Details {0}", ex);
            }
            finally
            {
                if (_reconnectionCancellationTokenSource != null)
                {
                    _reconnectionCancellationTokenSource.Dispose();
                    _reconnectionCancellationTokenSource = null;
                }
            }
        }

        public IQueryReader Query(string table, string whereClause, params string[] fields)
        {
            IBomQuery query = new BomQuery(_loggerFactory)
                .Using(ClientName)
                .Get(fields)
                .On(table)
                .Where(whereClause);

            string querySequence = ((BomQuery)query).GetString(Keys.SEQQR);

            var queryHandler = new QueryResponseHandler(_config.QueryTimeout);
            lock (_queryHandlers)
                _queryHandlers.Add(querySequence, queryHandler);

            Send(query as Bom);

            if (!queryHandler.Wait())
                throw new TimeoutException(
                    string.Format("The query with sequence number '{0}' has generated a timeout after {1} second(s)",
                                  querySequence, _config.QueryTimeout));

            FloorQueryResponse queryResult = _queryHandlers[querySequence].QueryResult;
            lock (_queryHandlers)
                _queryHandlers.Remove(querySequence);

            return new FloorServerQueryReader(queryResult);
        }


        public void QueryAsync(string table, string whereClause, params string[] fields)
        {
            IBomQuery query = new BomQuery(_loggerFactory)
                .Using(ClientName)
                .Get(fields)
                .On(table)
                .Where(whereClause);

            Send(query as Bom);
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Send(Bom message)
        {
            Send(message, false);
        }

        /// <summary>
        /// Registers the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="requiredFields">The fields required from the exception</param>
        public void Register(long exception, params string[] requiredFields)
        {
            Register(exception, string.Empty, false, requiredFields);
        }

        /// <summary>
        /// Registers the specified exception and specifies if the exception must be persisted by the FloorServer if the exception is not acknowledged.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="persisted">if set to <c>true</c> [persisted].</param>
        /// <param name="requiredFields">The required fields.</param>
        public void Register(long exception, bool persisted, params string[] requiredFields)
        {
            Register(exception, string.Empty, persisted, requiredFields);
        }

        /// <summary>
        /// Registers the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="persisted">if set to <c>true</c> [persisted].</param>
        /// <param name="requiredFields">The fields required from the exception</param>
        public void Register(long exception, string whereClause, bool persisted, params string[] requiredFields)
        {
            Register(exception, whereClause, persisted, ClientName, requiredFields);
        }

        /// <summary>
        /// Registers the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="persisted">if set to <c>true</c> [persisted].</param>
        /// <param name="clientName">The registering client name. Can be different from the current one</param>
        /// <param name="requiredFields">The fields required from the exception</param>
        public void Register(long exception, string whereClause, bool persisted, string clientName, params string[] requiredFields)
        {
            _logger.LogDebug("Registration requested for exception {0}",
                                                                     exception);
            Bom registerCommand = _commandBuilder.BuildRegisterCommand(exception, whereClause, persisted, clientName, requiredFields);
            if (registerCommand != null)
            {
                Send(registerCommand);
            }

            var newEx = new RegisteredException
            {
                ClientName = clientName,
                Persisted = persisted,
                RequiredFields = requiredFields,
                WhereClause = whereClause,
                ExceptionNum = exception
            };

            _registeredExceptions.Remove(newEx);
            _registeredExceptions.Add(newEx);

        }


        /// <summary>
        /// Unregisters the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        public void Unregister(long exception)
        {
            Unregister(exception, ClientName);
        }

        /// <summary>
        /// Unregisters the specified exception.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <param name="clientName">The unregistering client name. Can be different from the current on</param>
        public void Unregister(long exception, string clientName)
        {
            Bom unregisterCommand = _commandBuilder.BuildUnregisterCommand(exception, clientName);
            if (unregisterCommand != null)
            {
                if (exception == ExceptionKeys.ALL)
                {
                    _registeredExceptions.Clear();
                }
                else
                {
                    _registeredExceptions.RemoveAll(n => n.ClientName.Equals(clientName) && n.ExceptionNum == exception);
                }
                Send(unregisterCommand);
            }
        }

        /// <summary>
        /// Unregisters all.
        /// </summary>
        public void UnregisterAll()
        {
            Unregister(ExceptionKeys.ALL);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public virtual void Dispose()
        {
            if (_timer != null)
            {
                _timer.Stop();
                _timer.Dispose();
                _timer = null;
            }


            if (_bossClient != null)
                _bossClient.BossStatusChanged -= bossClient_BossStatusChanged;

            if (_bossReceiver != null)
            {
                _bossReceiver.FloorExceptionReceived -= bossReceiver_FloorExceptionReceived;
                _bossReceiver.QueryResponseReceived -= bossReceiver_QueryResponseReceived;
                _bossReceiver.FailedCommandResponseReceived -= bossReceiver_FailedCommandResponseReceived;
            }

            //remove UnregisterAll(); to keep FS backlog - client has to call UnregisterAll by themselve
            Close();

            if (_bossReceiver != null)
                _bossReceiver.Dispose();
            if (_bossTransmitter != null)
                _bossTransmitter.Dispose();
            if (_bossClient != null)
                _bossClient.Dispose();

            _bossReceiver = null;
            _bossTransmitter = null;
            _bossClient = null;
        }

        public void LockEGMs(string whereClause)
        {
            Bom message = _commandBuilder.BuildLockCommand(whereClause);
            if (message != null)
                Send(message);
        }

        public void UnlockEGMs(string whereClause)
        {
            Bom message = _commandBuilder.BuildUnlockCommand(whereClause);
            if (message != null)
                Send(message);
        }

        public void Reset(string whereClause)
        {
            Bom message = _commandBuilder.BuildResetCommand(whereClause);
            if (message != null)
                Send(message);
        }

        public void DestroyConfig(string whereClause)
        {
            Bom message = _commandBuilder.BuildDestroyConfigCommand(whereClause);
            if (message != null)
                Send(message);
        }

        public void CashoutEGMs(string whereClause, bool lockBeforeCashout)
        {
            Bom message = _commandBuilder.BuildCashoutCommand(whereClause, lockBeforeCashout);
            if (message != null)
                Send(message);
        }

        public void EnableMoneyIn(string whereClause)
        {
            Bom message = _commandBuilder.BuildEnableMoneyInCommand(whereClause);
            if (message != null)
                Send(message);
        }

        public void DisableMoneyIn(string whereClause)
        {
            Bom message = _commandBuilder.BuildDisableMoneyInCommand(whereClause);
            if (message != null)
                Send(message);
        }

        public void ClearPersistedCommands()
        {
            Bom msg = _commandBuilder.ClearPersistedCommands();
            if (msg != null)
                Send(msg);
        }

        public void SendWelcomeCreditsEligibilityCommand(string smHwId, string cardInSeqId, decimal cashAmount, decimal promoAmount, string promotionName, string playerLanguageCode)
        {
            Bom message = _commandBuilder.BuildWelcomeCreditsEligibilityCommand(smHwId, cardInSeqId, cashAmount, promoAmount, promotionName, playerLanguageCode);
            
            if (message != null)
            {
                Send(message);
            }
        }

        public void SendWelcomeCreditsOfferResponseCommand(string smHwId, string cardInSeqId, int resultCode, decimal? cashBalance, decimal? promoBalance)
        {
            Bom message = _commandBuilder.BuildWelcomeCreditsOfferResponseCommand(smHwId, cardInSeqId, resultCode, cashBalance, promoBalance);

            if (message != null)
            {
                Send(message);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="cardNumber">CardNumber has to be in hexadecimal format</param>
        /// <param name="pointsBalance"></param>
        /// <param name="whereClause"> </param>
        public void NotifyPlayerPointsUpdated(string cardNumber, double pointsBalance, string whereClause)
        {
            Bom updateCmd = _commandBuilder.BuildPointsUpdateCommand(cardNumber, pointsBalance, whereClause);
            if (updateCmd != null)
                Send(updateCmd);
        }


        public event EventHandler<EctEventArgs> EctResponseReceived;

        public IEctTransaction SendEctStartCommand(EctDestination destination, string transferId, long amount,
                                                   EctCreditType creditType, string playerCardNumber, string egmId,
                                                   string message, long timeout, int responseTimeout)
        {
            Bom ectCommand = _commandBuilder.BuildEctStartCommand(destination, transferId, amount, creditType,
                                                                  playerCardNumber, egmId, message, timeout);

            if (ectCommand == null)
                throw new InvalidOperationException("The Boss command cannot be built properly.");

            Register(ExceptionKeys.ECT_CTC,
                     Keys.ECT_AMOUNT,
                     Keys.ECT_RESULT,
                     Keys.PL_HOME_CASINO,
                     Keys.SMDBID,
                     Keys.PL_CARD_NR,
                     Keys.CARD_MEDIA_TYPE,
                     Keys.ECT_TRANSFER_ID);

            var ectResponse = new EctResponseHandler(responseTimeout);
            _ectHandlers.Add(transferId, ectResponse);

            Send(ectCommand);

            if (!ectResponse.WaitOne())
                throw new TimeoutException(
                    string.Format("No response has been received for the ECT_START command within {0} milliseconds",
                                  responseTimeout));

            IEctTransaction res = ectResponse.EctTransaction;
            _ectHandlers.Remove(res.TransferId);

            return res;
        }


        /// <summary>
        /// Sends a command to create a group of SM for an happy hour
        /// </summary>
        /// <param name="hhGroupId">The group id</param>
        /// <param name="hhGroupName">The group name</param>
        /// <param name="bonus">The bonus to apply during the happy hour</param>
        /// <param name="resourceId">The id of the person responsible for the creation of this group</param>
        /// <param name="startDate">The start date of the happy hour</param>
        /// <param name="endDate">The end date of the happy hour</param>
        public void SendCreateHappyHourGroupCommand(string hhGroupId, string hhGroupName, double bonus,
                                                    string resourceId, DateTime startDate, DateTime endDate)
        {
            Bom msg = _commandBuilder.BuildCreateHappyHourGroupCommand(hhGroupId, hhGroupName, bonus, resourceId,
                                                                       startDate, endDate);
            if (msg != null)
                Send(msg);
        }

        /// <summary>
        /// Sends a command to add a slot machine to an happy hour group
        /// </summary>
        /// <param name="hhGroupId">The id of the group</param>
        /// <param name="smHwId">The hardware id of the SM</param>
        public void SendAddSMToGroupCommand(string hhGroupId, string smHwId)
        {
            Bom msg = _commandBuilder.BuildAddEGMToGroupCommand(hhGroupId, smHwId);
            if (msg != null)
                Send(msg);
        }

        /// <summary>
        /// Sends a command to remove an happy hour group
        /// </summary>
        /// <param name="hhGroupId">The id of the group</param>
        public void SendRemoveHappyHourGroupCommand(string hhGroupId)
        {
            Bom msg = _commandBuilder.BuildDeleteHappyHourGroupCommand(hhGroupId);
            if (msg != null)
                Send(msg);
        }

        /// <summary>
        /// Sends a command to remove a SM from an happy hour group
        /// </summary>
        /// <param name="hhGroupId">The id of the group</param>
        /// <param name="smHwId">The hardware id of the SM</param>
        public void SendRemoveSMFromHappyHourGroupCommand(string hhGroupId, string smHwId)
        {
            Bom msg = _commandBuilder.BuildDeleteEGMFromGroupCommand(hhGroupId, smHwId);
            if (msg != null)
                Send(msg);
        }


        /// <summary>
        /// Clear all periodic readings for a readName.
        /// </summary>
        /// <param name="readName">Destination (client) for which clearing the readings.</param>
        public void ClearPeriodicReading(string readName)
        {
            Bom msg = _commandBuilder.BuildClearReadingCommand(readName);
            if (msg != null)
                Send(msg);
        }

        /// <summary>
        /// Builds the periodic reading command.
        /// </summary>
        /// <param name="periodicReadingCommand">the periodic reading command.</param>
        public void CreatePeriodicReading(PeriodicReadingCommand periodicReadingCommand)
        {
            Bom msg = _commandBuilder.BuildPeriodicReadingCommand(periodicReadingCommand.ReadName,
                                                                  periodicReadingCommand.ReadID,
                                                                  periodicReadingCommand.ReadRepeat,
                                                                  periodicReadingCommand.ReadOffset, 0,
                                                                  periodicReadingCommand.WhereClause,
                                                                  periodicReadingCommand.Persisted,
                                                                  periodicReadingCommand.SendTerminatorException,
                                                                  periodicReadingCommand.ReadedFields.ToArray());
            if (msg != null)
                Send(msg);
        }

        /// <summary>
        /// Builds the periodic reading command.
        /// </summary>
        /// <param name="periodicReadingCommands">the periodic reading command list.</param>
        /// <param name="persistCommand">if set to <c>true</c> [persisted].</param>
        ///         
        public void CreatePeriodicReadings(List<PeriodicReadingCommand> periodicReadingCommands, bool persistCommand)
        {
            if (!persistCommand)
            {
                foreach (PeriodicReadingCommand periodicReadingCommand in periodicReadingCommands)
                    CreatePeriodicReading(periodicReadingCommand);
            }
            else
            {
                List<Bom> bomMessages =
                    periodicReadingCommands.Select(
                        periodicReadingCommand =>
                        _commandBuilder.BuildPeriodicReadingCommand(periodicReadingCommand.ReadName,
                                                                    periodicReadingCommand.ReadID,
                                                                    periodicReadingCommand.ReadRepeat,
                                                                    periodicReadingCommand.ReadOffset, 0,
                                                                    periodicReadingCommand.WhereClause,
                                                                    periodicReadingCommand.Persisted,
                                                                    periodicReadingCommand.SendTerminatorException,
                                                                    periodicReadingCommand.ReadedFields.ToArray())).
                        ToList();
                SendPersistedCommands(bomMessages);
            }
        }

        private void SendPersistedCommands(List<Bom> bomMessages)
        {
            Bom rootBomMessage = _commandBuilder.BuildPeristedCommandBody();
            var mainValues = _bomFactory.CreateBom();
            rootBomMessage.PutMsg(Tags.VALUES, mainValues);

            int currentIndex = 1;
            foreach (Bom bomMessage in bomMessages)
            {
                mainValues.PutMsg(currentIndex++.ToString(), bomMessage);
            }

            if (rootBomMessage != null)
                Send(rootBomMessage);
        }


        /// <summary>
        /// Sends the live message.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="messageID">The message ID.</param>
        /// <param name="whereClause"> </param>
        /// <param name="cmodLanguageCode"> </param>
        public void SendLiveMessage(LiveMessagingFlags flags, ushort messageID, string whereClause,
                                    string cmodLanguageCode)
        {
            SendLiveMessage(flags, messageID, whereClause, string.Empty, cmodLanguageCode);
        }

        /// <summary>
        /// Sends the live message.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="messageID">The message ID.</param>
        /// <param name="whereClause"> </param>
        /// <param name="text">The text.</param>
        /// <param name="cmodLanguageCode"> </param>
        public void SendLiveMessage(LiveMessagingFlags flags, ushort messageID, string whereClause, string text,
                                    string cmodLanguageCode)
        {
            SendLiveMessage(flags, messageID, whereClause, text, cmodLanguageCode, -1);
        }

        /// <summary>
        /// Sends the live message.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="messageID">The message ID.</param>
        /// <param name="whereClause"> </param>
        /// <param name="text">The text.</param>
        /// <param name="cmodLanguageCode"> </param>
        /// <param name="duration">The duration.</param>
        public void SendLiveMessage(LiveMessagingFlags flags, ushort messageID, string whereClause, string text,
                                    string cmodLanguageCode, int duration)
        {
            SendLiveMessage(flags, messageID, whereClause, text, cmodLanguageCode, duration, string.Empty);
        }

        /// <summary>
        /// Sends the live message.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="messageID">The message ID.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="text">The text.</param>
        /// <param name="cmodLanguageCode"> </param>
        /// <param name="duration">The duration.</param>
        /// <param name="cardNumber">The card number.</param>
        public void SendLiveMessage(
            LiveMessagingFlags flags,
            ushort messageID,
            string whereClause,
            string text,
            string cmodLanguageCode,
            int duration,
            string cardNumber)
        {
            SendLiveMessage(flags, messageID, whereClause, text, cmodLanguageCode, duration, cardNumber, messageID);
        }

        /// <summary>
        /// Sends the live message.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <param name="messageID">The message ID.</param>
        /// <param name="whereClause">The where clause.</param>
        /// <param name="text">The text.</param>
        /// <param name="cmodLanguageCode"> </param>
        /// <param name="duration">The duration.</param>
        /// <param name="cardNumber">The card number.</param>
        /// <param name="hostMessageID">The host message ID.</param>
        public void SendLiveMessage(
            LiveMessagingFlags flags,
            ushort messageID,
            string whereClause,
            string text,
            string cmodLanguageCode,
            int duration,
            string cardNumber,
            ulong hostMessageID)
        {
            // Remove carridge return
            text = text.Replace(Environment.NewLine, @"\n");
            text = text.Replace("\n", @"\n");
            Bom liveMessageCommand = _commandBuilder.BuildLiveMessageCommand(flags, messageID, whereClause, text,
                                                                             cmodLanguageCode, duration, cardNumber,
                                                                             hostMessageID);
            if (liveMessageCommand != null)
                Send(liveMessageCommand);
        }

        public void SendLiveMessage(LiveMessagingFlags flags, string priority, ushort messageID, string buttonText,
                                    string button2Text, string text, string cmodLanguageCode, int duration,
                                    string resources, string templateId, string withAnswer,
                                    string cardNumber, ulong hostMessageID, string whereClause)
        {
            // Remove carridge return
            text = text.Replace(Environment.NewLine, @"\n");
            text = text.Replace("\n", @"\n");
            Bom liveMessageCommand = _commandBuilder.BuildLiveMessageCommand(flags, priority, messageID, buttonText,
                                                                             button2Text, text, cmodLanguageCode,
                                                                             duration, resources, templateId, withAnswer,
                                                                             cardNumber, hostMessageID, whereClause);
            if (liveMessageCommand != null)
                Send(liveMessageCommand);
        }


        /// <summary>
        /// Sends the ticket redemption command according to the parameters sent by the Ticket System.
        /// </summary>
        /// <param name="validationNumber">The validation number.</param>
        /// <param name="voucherTransferCode">The voucher transfer code.</param>
        /// <param name="amountCents">The amount cents.</param>
        /// <param name="restrictedExpiration">The restricted expiration.</param>
        /// <param name="restrictedPoolID">The restricted pool ID.</param>
        /// <param name="whereClause">The where clause.</param>
        public void SendTicketRedemptionCommand(string validationNumber, short voucherTransferCode, long amountCents,
                                                long restrictedExpiration, int restrictedPoolID, string whereClause)
        {
            Bom ticketRedemptionCommand = _commandBuilder.BuildTicketRedemptionCommand(validationNumber,
                                                                                       voucherTransferCode, amountCents,
                                                                                       restrictedExpiration,
                                                                                       restrictedPoolID, whereClause);
            if (ticketRedemptionCommand != null)
                Send(ticketRedemptionCommand);
        }

        /// <summary>
        /// Sends the ticket issue command.
        /// </summary>
        /// <param name="validationNumber">The validation number.</param>
        /// <param name="validationSystemID"></param>
        /// <param name="whereClause">The where clause.</param>
        public void SendTicketIssueCommand(string validationNumber, short validationSystemID, string whereClause)
        {
            Bom ticketIssueCommand = _commandBuilder.BuildTicketIssueCommand(validationNumber, validationSystemID,
                                                                             whereClause);
            if (ticketIssueCommand != null)
                Send(ticketIssueCommand);
        }

        public void SendTicketValidationParameters(long validationID, long validationSequenceNbr, string whereClause)
        {
            Bom enhancedParams = _commandBuilder.BuildTicketValidationParametersCommand(validationID,
                                                                                        validationSequenceNbr,
                                                                                        whereClause);
            if (enhancedParams != null)
                Send(enhancedParams);
        }

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
        public void SendTicketConfigUpdateCommand(string shortName, string addressLine1, string addressLine2,
                                                  string restrictedText, string debitText, int expiresMachineAfter,
                                                  int expiresPromotionalAfter, ValidationControlFlags controlFlags,
                                                  string whereClause)
        {
            Bom ticketConfigUpdate = _commandBuilder.BuildTicketConfigUpdateCommand(shortName, addressLine1,
                                                                                    addressLine2, restrictedText,
                                                                                    debitText, expiresMachineAfter,
                                                                                    expiresPromotionalAfter,
                                                                                    controlFlags, whereClause);
            if (ticketConfigUpdate != null)
                Send(ticketConfigUpdate);
        }

        public void SendTicketSystemStatusCommand(bool isConnected)
        {
            Bom ticketStatus = _commandBuilder.BuildTicketSystemStatusCommand(isConnected);
            if (ticketStatus != null)
                Send(ticketStatus);
        }

        /// <summary>
        /// Connects this instance to the FloorServer.
        /// </summary>
        private bool Connect(bool isReconnection)
        {
            Status = ConnectionStatus.Connecting;
            try
            {
                if (_bossClient != null)
                    _bossClient.Stop();

                _bossClient = new BossClient(_config.EndPoint.Address.ToString(), _config.EndPoint.Port, _bomFactory, _loggerFactory);
                _bossClient.SetName(ClientName);
                _bossClient.BossStatusChanged += bossClient_BossStatusChanged;
                _bossTransmitter = new BossTransmitter(_bossClient, _commandBuilder, AckEnabled, _loggerFactory);
                _bossReceiver = new BossReceiver(_bossClient, _commandBuilder, _loggerFactory);
                _bossReceiver.FloorExceptionReceived += bossReceiver_FloorExceptionReceived;
                _bossReceiver.QueryResponseReceived += bossReceiver_QueryResponseReceived;
                _bossReceiver.FailedCommandResponseReceived += bossReceiver_FailedCommandResponseReceived;

                _logger.LogDebug(
                    "FS client ({0}) is connecting to {1}:{2} ...", ClientName,
                    _config.EndPoint.Address.ToString(), _config.EndPoint.Port);
                _bossClient.Start();
                _bossClient.KeepAlive = true;
            }
            catch (Exception e)
            {
                _status = ConnectionStatus.Disconnected;
                _logger.LogError(e,
                    "FS with IP address {0}:{1} is not reachable.", _config.EndPoint.Address.ToString(),
                    _config.EndPoint.Port);
                if (!isReconnection)
                    TryReconnect();
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reconnects this instance.
        /// </summary>
        private void Reconnect(object obj)
        {
            CancellationToken token = (CancellationToken)obj;
            while (!token.IsCancellationRequested && !Connect(true))
            {
                Thread.Sleep(_config.ReconnectionDelay);
            }
        }

        /// <summary>
        /// Tries the reconnect.
        /// </summary>
        private void TryReconnect()
        {
            if (_reconnectionCancellationTokenSource != null)
            {
                _reconnectionCancellationTokenSource.Dispose();
                _reconnectionCancellationTokenSource = null;
            }

            _reconnectionCancellationTokenSource = new CancellationTokenSource();
            var reconnectionThread = new Thread(new ParameterizedThreadStart(Reconnect)) {IsBackground = true};
            reconnectionThread.Start(_reconnectionCancellationTokenSource.Token);
        }

        /// <summary>
        /// Handles the FloorExceptionReceived event of the bossReceiver control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Boss.Communication.BomEventArgs"/> instance containing the event data.</param>
        private void bossReceiver_FloorExceptionReceived(object sender, CancelableBomEventArgs e)
        {
            Bom msg = e.Message;
            long exception = msg.GetLong(Tags.EXC);

            switch (exception)
            {
                case ExceptionKeys.EXC_FS_CONNECTION:
                case ExceptionKeys.EXC_FS_RECONNECTION:
                    if (e.Message.GetString(Tags.DST).Equals(ClientName))
                    {
                        Status = ConnectionStatus.Connected;
                        Bom helloCommand = _commandBuilder.BuildHelloCommand(_config.Winsize);
                        if (helloCommand != null)
                            Send(helloCommand, true);
                    }
                    break;
                case ExceptionKeys.ECT_CTC:
                    var ectTransaction = new EctTransaction(FloorExceptionHelper.ConvertToGenericFloorEvent(msg));
                    if (ectTransaction.TransferId != null && _ectHandlers.ContainsKey(ectTransaction.TransferId))
                    {
                        EctResponseHandler ectResponse = _ectHandlers[ectTransaction.TransferId];
                        if (ectResponse != null)
                        {
                            ectResponse.EctTransaction = ectTransaction;
                            ectResponse.Set();
                            //// Send a new event here (managing case when event received after ECT response timeout)
                            _logger.LogDebug(
                                "ECT Response has been received. Transfer ID = {0}", ectTransaction.TransferId);
                            if (_ectHandlers[ectTransaction.TransferId].HasBeenTimeout)
                            {
                                _logger.LogInformation(
                                    "ECT Response with transfer ID = {0} had been timeout before",
                                    ectTransaction.TransferId);
                                if (EctResponseReceived != null)
                                {
                                    EctResponseReceived(this, new EctEventArgs(ectResponse.EctTransaction));
                                }

                                _ectHandlers.Remove(ectTransaction.TransferId);
                            }
                        }
                    }
                    else
                    {
                        HandleDefaultExcpetion(msg, e);
                    }
                    break;
                default:
                    HandleDefaultExcpetion(msg, e);
                    break;
            }
        }

        private void HandleDefaultExcpetion(Bom msg, CancelableBomEventArgs e)
        {
            FloorEventArgs args = FloorExceptionHelper.ConvertToGenericFloorEvent(msg);
            OnExceptionReceived(args);
            e.Cancel = args.CancelMessage;
        }

        private void bossReceiver_FailedCommandResponseReceived(object sender, BomEventArgs e)
        {
            Bom msg = e.Message;
            string cmdNumber = msg.GetString(Keys.CMD);
            _logger.LogError(
                string.Format("An ERROR ack was received from the Floor Server for command {0} with message : {1}",
                              cmdNumber, msg.GetString(Tags.ERROR)));
            if (cmdNumber == CommandKeys.CMD_ECT_START)
            {
                var ectTransaction = new EctTransaction(msg.GetMsg(Keys.VALUES).GetString(Keys.ECT_TRANSFER_ID),
                                                        EctResultCodes.ERR_CM_ACK, 0, "", msg.GetString(Keys.SMDBID),
                                                        msg.GetMsg(Keys.VALUES).GetString(Keys.ECT_CARD_NR));
                EctResponseHandler ectResponse = _ectHandlers[ectTransaction.TransferId];
                if (ectResponse != null)
                {
                    ectResponse.EctTransaction = ectTransaction;
                    ectResponse.Set();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void bossReceiver_QueryResponseReceived(object sender, BomEventArgs e)
        {
            Bom msg = e.Message;
            string querySequence = msg.GetString(Keys.SEQQR);

            if (_queryHandlers.ContainsKey(querySequence))
            {
                QueryResponseHandler queryHandler = _queryHandlers[querySequence];
                queryHandler.QueryResult = new FloorQueryResponse(msg);
                queryHandler.Set();
            }

            OnQueryResponseReceived(new FloorQueryArgs(msg));
        }


        /// <summary>
        /// Handles the BossStatusChanged event of the bossClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Boss.Communication.BossStatusEventArgs"/> instance containing the event data.</param>
        private void bossClient_BossStatusChanged(object sender, BossStatusEventArgs e)
        {
            if (e == null) return;
            switch (e.Type)
            {
                case BossEventType.NewName:
                    // renaming is done in the BossClient automatically
                    break;
                case BossEventType.LinkDown:
                    if (Status == ConnectionStatus.Closed)
                        break;
                    if (Status == ConnectionStatus.Connected)
                    {
                        Status = ConnectionStatus.Broken;
                        TryReconnect();
                    }
                    break;
                case BossEventType.Error:
                    _logger.LogDebug("Error received from FloorServer : {0}",  e.Message);
                    break;
            }
        }

        /// <summary>
        /// Sends the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="force">if set to <c>true</c> [force].</param>
        public void Send(Bom message, bool force)
        {
            if (_bossClient == null)
            {
                _logger.LogError("Unable to send command: Please connect first.");
                return;
            }

            if (!force && Status != ConnectionStatus.Connected)
            {
                EnqueueMessage(message);
                return;
            }
            _bossTransmitter.SendMessage(message);
        }

        /// <summary>
        /// Sends the queued messages.
        /// </summary>
        public void SendQueuedMessages()
        {
            while (_queuedMessages.Count > 0)
            {
                Send(_queuedMessages.Dequeue());
            }
        }

        /// <summary>
        /// Adds the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void EnqueueMessage(Bom message)
        {
            if (message != null)
            {
                _queuedMessages.Enqueue(message);
                _logger.LogDebug("Enqueue message : {0}.", message.ToString());
            }
        }

        public void CheckRegistration()
        {
            var clientNames = _registeredExceptions.Select(n => n.ClientName).Distinct().ToList();
            var whereClause = FloorCommandHelper.Equal(Fields.NAME, clientNames);

            var fields = new string[]
            {
                "MSG_REQU",
                "NAME",
                "ACK_REQU",
                "WHERE_REQU",
                "FIELDS_REQU",
                "DONT_SEND",
                "EXC_ID",
                "autosend",
                "desc",
                "where",
                "workfields"
            };

            var fsSubscribedException = new List<RegisteredException>();
            var r = Query("CLIENTS", whereClause, fields);
            while (r.Read())
            {
                var row = r.Current;
                var name = row["NAME"];

                var excFields = row["FIELDS_REQU"].Split(' ');

                long exc;
                if (!long.TryParse(row["MSG_REQU"], out exc)) continue;

                fsSubscribedException.Add(new RegisteredException
                {
                    ClientName = name,
                    Persisted = "1".Equals(row["ACK_REQU"]),
                    RequiredFields = fields,
                    WhereClause = row["WHERE_REQU"],
                    ExceptionNum = exc
                });
            }

            RegisteredException[] previouslyRegistered = _registeredExceptions.ToArray();


            foreach (var prev in previouslyRegistered)
            {
                if (!fsSubscribedException.Exists(n => n.ExceptionNum.Equals(prev.ExceptionNum) && n.ClientName.Equals(prev.ClientName)))
                {
                    _logger.LogWarning("Exception {0} was not registered on {1}", prev.ExceptionNum, prev.ClientName);
                    Register(prev.ExceptionNum, prev.WhereClause, prev.Persisted, prev.ClientName, prev.RequiredFields);
                    _logger.LogWarning("Exception {0} now registered", prev.ExceptionNum);
                }
            }
        }

    }
}