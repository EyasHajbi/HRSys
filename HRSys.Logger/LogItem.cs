using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.Logger
{
    public class LogItem
    {
        public LogItem()
        {
            Timestamp = DateTime.Now;
            AdditionalInfo = new Dictionary<string, object>();
        }

        public DateTime Timestamp { get; private set; }

        public string Message { get; set; }

        public string UserId { get; set; }

        public string UserName { get; set; }

        public long? ElapsedMilliseconds { get; set; }

        public Exception Exception { get; set; }

        public Dictionary<string, object> AdditionalInfo { get; set; }
    }
}
