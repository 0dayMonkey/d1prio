#region

using System;
using System.Collections.Generic;
using System.Threading;
using FloorServer.Client.Boss;
using FloorServer.Client.Tools;
using Microsoft.Extensions.Logging;

#endregion

namespace FloorServer.Client.Communication
{
    public class BossTransmitter : IDisposable
    {
        private Dictionary<Bom, RetryCounter> followedMessages;

        private Thread transmitterThread;
        private ThreadStart transmitterThreadStart;
        private bool acknowledgeMode;

        private BossClient _bossClient;
        private BossCommandBuilder _commandBuilder;
        private static int sequenceNumber = 0;
        private bool running;
        private double timeout = 2000;
        private int timeoutIncrement = 50;
        private int maxRetry = 3;

        private readonly ILogger _logger;

        public int MaxRetry
        {
            get { return maxRetry; }
            set { maxRetry = value; }
        }

        #region Properties

        public static int SequenceNumber
        {
            get { return sequenceNumber++; }
        }

        public bool WaitForAck
        {
            get { return acknowledgeMode; }
            set
            {
                acknowledgeMode = value;
                if (acknowledgeMode)
                {
                    this._bossClient.BossMessageReceived += new EventHandler<BomEventArgs>(bossClient_BossMessageReceived);
                    this.transmitterThreadStart = new ThreadStart(Run);
                    this.transmitterThread = new Thread(transmitterThreadStart);
                    this.transmitterThread.Start();
                }
                else
                    this._bossClient.BossMessageReceived -= new EventHandler<BomEventArgs>(bossClient_BossMessageReceived);
            }
        }

        public double Timeout
        {
            get { return timeout; }
            set { timeout = value; }
        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="BossTransmitter"/> class.
        /// </summary>
        /// <param name="bossClient">The boss client.</param>
        public BossTransmitter(BossClient bossClient, BossCommandBuilder commandBuilder, bool waitForAck, ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BossTransmitter>();
            if (bossClient == null)
            {
                _logger.LogError("Unable to initialize BossTransmitter : BossClient cannot be null.");
                throw new ArgumentNullException();
            }

            //initializing queues
            
            followedMessages = new Dictionary<Bom, RetryCounter>();
            _bossClient = bossClient;
            WaitForAck = waitForAck;
            _commandBuilder = commandBuilder;
            running = true;
        }

        

        /// <summary>
        /// Handles the BossMessageReceived event of the bossClient control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="Boss.Communication.BomEventArgs"/> instance containing the event data.</param>
        private void bossClient_BossMessageReceived(object sender, BomEventArgs e)
        {
            Bom acknowledgedMessage = null;
            if (e.Message != null)
            {
                bool ackValid = false;
                IEnumerator<Bom> keyEnum = followedMessages.Keys.GetEnumerator();
                while (!ackValid && keyEnum.MoveNext())
                {
                    if (BomTools.IsAckValid(keyEnum.Current, e.Message))
                    {
                        acknowledgedMessage = keyEnum.Current;
                        ackValid = true;
                    }
                }
                if (acknowledgedMessage != null)
                {
                    _logger.LogDebug("ACK Received: SequenceNumber = {0}", acknowledgedMessage.GetString(Keys.SEQ_NBR));
                    followedMessages.Remove(acknowledgedMessage);
                }
            }
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        private void Run()
        {
            while (running && (WaitForAck || followedMessages.Count > 0))
            {
                List<Bom> messagesToDelete = new List<Bom>();
                foreach (KeyValuePair<Bom, RetryCounter> entry in followedMessages)
                {
                    if (DateTime.Now.CompareTo(entry.Value.Timestamp) >= 0)
                    {
                        if (entry.Value.RetryNumber < MaxRetry)
                        {
                            entry.Value.RetryNumber++;
                            SendMessage(entry.Key);
                        }
                        else
                        {
                            _logger.LogInformation("Message marked for deletion: {0}", entry.Key.ToString());
                            messagesToDelete.Add(entry.Key);
                        }
                    }
                }

                //remove all elements marked for deletion
                foreach (Bom key in messagesToDelete)
                    followedMessages.Remove(key);

                //clear the list
                messagesToDelete.Clear();

                Thread.Sleep(timeoutIncrement);
            }
        }

        

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        public bool SendMessage(Bom message)
        {
            if (message != null)
            {
                if (message.GetString(Keys.ACT) != Keys.QUERY)
                {
                    _commandBuilder.AppendSequenceNumber(message, BossTransmitter.SequenceNumber);
                    if (WaitForAck)
                        followedMessages[message] = new RetryCounter(DateTime.Now.AddMilliseconds(Timeout));
                }
                _logger.LogDebug("Sending command : {0}", message.ToString());
                _bossClient.Write(message.ToString());
                return true;
            }
            return false;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.running = false;
        }

        #endregion
    }
}
