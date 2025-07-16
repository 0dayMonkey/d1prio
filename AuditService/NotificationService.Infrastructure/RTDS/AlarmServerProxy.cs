using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Domain.Messages;
using NotificationService.Infrastructure.Messages;
using NotificationService.Infrastructure.RTDS.KeyValueTools;
using System;

using System.Threading.Tasks;

namespace NotificationService.Infrastructure.RTDS
{
    public class AlarmServerProxy : RTDSProxy, IMessageSender
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<AlarmServerProxy> _logger;

        protected override string Server_address => _configuration["SERVICE:AlarmServer:address"];
        protected override string Server_port => _configuration["SERVICE:AlarmServer:port"];

        public AlarmServerProxy(IConfiguration configuration, ILogger<AlarmServerProxy> logger, ILogger<RTDSProxy> rtdsLogger)
            :base(rtdsLogger)
        {
            _configuration = configuration;
            _logger = logger;
        }
     

        public async Task<bool> SendMessage(NotificationMessage message, MetaData? metaData = null)
        {
            if (message == null) { throw new ArgumentNullException(nameof(message)); }
            if(message.Channels.AlarmMessage == null)
            {
                return false;
            }
            
            if (_writer == null)
            {
                _logger.LogError("Writer is null");
                return false;
            }

            if (_state != ConnectionState.Initialized)
            {
                _logger.LogError("Connection not initialized");
                return false;
            }
            
            var alarmMessageInfo = message.Channels.AlarmMessage;
            var kvMessage = new KeyValueMap();
            kvMessage.AddStrings("MSG_TYPE", "ONLINE_MSG");
            kvMessage.AddStrings("CASINO_ID", _configuration["CFG:CASINO_ID"]);
            kvMessage.AddStrings("OL_MSG_COD", MaxSize(alarmMessageInfo.OnlineMessageCode, 15));
            kvMessage.AddStrings("OBJ_TYP", "6");

            var olParams = new KeyValueMap();
            foreach (var kv in message.KeyValues)
            {
                if (kv.Value != null && kv.Key != null)
                {
                    olParams.AddStrings(kv.Key, kv.Value);
                }
            }
            kvMessage.AddKVMap("PARAM_VAL", olParams);

            kvMessage.AddStrings("USER_COD", "");
            kvMessage.AddStrings("STATION_COD", "");
            kvMessage.AddStrings("APP_COD", "SEC01");
            kvMessage.AddStrings("OBJ_COD", "");
            kvMessage.AddStrings("MODULE_ID", "NOTIFICATIONSERVICE");
            kvMessage.AddStrings("PGM_COD", "");
            kvMessage.AddStrings("SEQ_NBR", $"C{_currentSequence++}");
            kvMessage.AddStrings("TIMESTAMP", FormatTimeStamp(message.MessageTimestamp));

            kvMessage.AddStrings("TEMPLATE", MaxSize(message.Template, 500));
            kvMessage.AddStrings("AUTO_CREATE", "TRUE");
            kvMessage.AddStrings("RETENTION_DAYS", alarmMessageInfo.RetentionDays.ToString());
            kvMessage.AddStrings("BACKGROUND_COLOR", alarmMessageInfo.BackgroundColor ?? "#FFFFFF");
            kvMessage.AddStrings("FOREGROUND_COLOR", alarmMessageInfo.ForegroundColor ?? "#000000");
            kvMessage.AddStrings("ACK_REQUIRED", alarmMessageInfo.AckRequired ? "TRUE" : "FALSE");
            kvMessage.AddStrings("MESSAGE_NAME", MaxSize(message.Subject,30));
            kvMessage.AddStrings("SEVERITY", ((int)message.Severity).ToString());
            
            if(_currentName != null) kvMessage.AddStrings("CLIENT_ID", _currentName);

            _state = ConnectionState.Sending;
            _writer.Write(kvMessage.ToString() + "\r");
            _writer.Flush();
            _tcsSendingMessage = new TaskCompletionSource<bool>();

            var timeOutTask = Task.Delay(DefaultTimeout);
            await Task.WhenAny(timeOutTask, _tcsSendingMessage.Task);

            if (timeOutTask.IsCompleted)
            {
                _logger.LogError("Alarm server did not reply in time when sending message");
                _state = ConnectionState.Initialized;
                return false;
            }

            return _tcsSendingMessage.Task.Result;
        }

        private static string MaxSize(string source, int maxSize)
        {
            return source.Length <= maxSize ? source : source[..maxSize];
        }

        private static string FormatTimeStamp(DateTime dateTime)
        {
            var year = dateTime.Year;
            var month = dateTime.Month;
            var day = dateTime.Day;
            var hour = dateTime.Hour;
            var minute = dateTime.Minute;
            var second = dateTime.Second;
            var millisecond = dateTime.Millisecond;

            return $"{year:d4}{month:d2}{day:d2}{hour:d2}{minute:d2}{second:d2}{millisecond:d3}";
        }
    }
}
