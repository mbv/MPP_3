using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{
    public class LogMessage
    {
        public LogLevel Level { get; set; }
        public string Message { get; set; }

        private readonly string _time;

        public LogMessage(LogLevel level, string message)
        {
            this.Level = level;
            this.Message = message;
            _time = DateTime.Now.ToString();
        }

        public string ConvertToString()
        {
            return $"[ {_time} ] {Level} {Message}{Environment.NewLine}";
        }
    }
}
