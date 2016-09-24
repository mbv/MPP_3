using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{
    public class LogsFactory
    {
        private readonly Logger _logger;

        public LogsFactory(Logger logger)
        {
            this._logger = logger;
        }

        public void Create(int logsCount, LogLevel level)
        {
            for (int i = 0; i < logsCount; i++)
                _logger.Log(new LogMessage(level, "task " + i));
            _logger.FlushRemainLogs();
        }
    }
}