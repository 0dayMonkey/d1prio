using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationService.Domain.Messages
{
    public class AlarmMessageInfo
    {
        public int RetentionDays { get; set; }
        public string BackgroundColor { get; set; } = string.Empty;
        public string ForegroundColor { get; set; } = string.Empty;
        public bool AckRequired { get; set; }        
        public string OnlineMessageCode { get; set; } = string.Empty;
    }
}
