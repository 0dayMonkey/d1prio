#region

using System;
using FloorServer.Client.Boss;
using Microsoft.Extensions.Logging;

#endregion

namespace FloorServer.Client.Communication
{
    public class BossReceiver : IDisposable
    {
        private BossCommandBuilder _commandBuilder;
        private ILogger _logger;
        private long nextExceptionNumber = 0;
        private long nextQueryNumber = 0;

        #region events

        public event EventHandler<CancelableBomEventArgs> FloorExceptionReceived;
        public event EventHandler<BomEventArgs> QueryResponseReceived;
        public event EventHandler<BomEventArgs> FailedCommandResponseReceived;

        /// <summary>
        /// Raises the <see cref="E:FloorExceptionReceived"/> event.
        /// </summary>
        /// <param name="e">The <see cref="Boss.Communication.BomEventArgs"/> instance containing the event data.</param>
        protected void OnFloorExceptionReceived(CancelableBomEventArgs e)
        {
            if (FloorExceptionReceived != null)
                FloorExceptionReceived(this, e);
        }

        protected void OnQueryResponseReceived(BomEventArgs e)
        {
            if (QueryResponseReceived != null)
                QueryResponseReceived(this, e);
        }

        protected void OnFailedCommandResponseReceived(BomEventArgs e)
        {
            if (FailedCommandResponseReceived != null)
                FailedCommandResponseReceived(this, e);
        }

        #endregion

        private BossClient bossClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="BossReceiver"/> class.
        /// </summary>
        /// <param name="bossClient">The boss client.</param>
        public BossReceiver(BossClient bossClient, BossCommandBuilder commandBuilder, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BossReceiver>();
            if (bossClient == null)
            {
                _logger.LogDebug("Unable to initialize BossReceiver: parameters cannot be null.");
                throw new ArgumentNullException();
            }

            this._commandBuilder = commandBuilder;
            this.bossClient = bossClient;
            this.bossClient.BossMessageReceived += new EventHandler<BomEventArgs>(bossClient_BossMessageReceived);
        }

        private void SendAck(Bom msg)
        {
            var ack = _commandBuilder.BuildAcknowledge(msg).ToString();
            _logger.LogDebug("Sending ACK: {0}", ack);
            this.bossClient.Write(ack);
        }

        /// <summary>
        /// Handles the BossMessageReceived event of the bossClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Boss.Communication.BomEventArgs"/> instance containing the event data.</param>
        private void bossClient_BossMessageReceived(object sender, BomEventArgs e)
        {
            Bom msg = e.Message;
            if (msg == null) return;

            _logger.LogDebug("Boss message received : {0}", msg.ToString());
            //do some filters
            if (msg.ContainsKey(Tags.ACT) && (msg.GetString(Tags.ACT).CompareTo(Tags.EXC) == 0))
            {
                if (msg.ContainsKey(Tags.EXC_NUM))
                {
                    var excNum = msg.GetLong(Tags.EXC_NUM);
                    if (excNum < nextExceptionNumber)
                    {
                        _logger.LogWarning("Ignore exception with EXC_NUM {0} (Lower than expected {1})", msg.GetLong(Tags.EXC_NUM), nextExceptionNumber);
                        SendAck(msg);
                        return;
                    }
                }

                CancelableBomEventArgs cancelableEventArgs = null;

                try
                {
                    //We raise the event and then check if the ack needs to be sent
                    cancelableEventArgs = new CancelableBomEventArgs(e.Message);

                    OnFloorExceptionReceived(cancelableEventArgs);
                }
                catch (Exception mainException)
                {
                    _logger.LogError(mainException, "An error occured where handling Floor exception");
                }
                finally
                {
                    if (cancelableEventArgs != null)
                    {
                        if (!cancelableEventArgs.Cancel)
                        {
                            SendAck(msg);
                            this.nextExceptionNumber = msg.GetLong(Tags.EXC_NUM) + 1;
                        }
                        else
                        {
                            _logger.LogDebug("Canceled ACK for EXC: {0}", cancelableEventArgs.Message.ToString());
                        }
                    }
                }
            }
            else if (msg.ContainsKey(Tags.ACT) && (msg.GetString(Tags.ACT).CompareTo(Tags.QUERY) == 0))
            {
                if (msg.ContainsKey(Keys.SEQQR))
                {
                    long queryNum = msg.GetLong(Keys.SEQQR);
                    if (queryNum < nextQueryNumber)
                    {
                        _logger.LogDebug("Ignore duplicate query response with SEQQR {0}", queryNum);
                        return;
                    }
                    if (queryNum > nextQueryNumber)
                    {
                        _logger.LogDebug("Adjusting expected SEQQR from {0} to {1}", nextQueryNumber, queryNum + 1);
                        nextQueryNumber = queryNum + 1;
                    }
                    else
                        nextQueryNumber++;
                    OnQueryResponseReceived(e);
                }
            }
            //In case of ECT command that fails, we will receive an ERR Ack from the FS that will be managed by this piece of code. TODO : Manage ack correctly
            else if (msg.ContainsKey(Tags.ACT) && (msg.GetString(Tags.ACT).CompareTo(Tags.CMD) == 0) && msg.GetString(Tags.STATUS).Equals("ERR"))
            {
                OnFailedCommandResponseReceived(e);
            }
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

        }

        #endregion
    }
}
