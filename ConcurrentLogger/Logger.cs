using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConcurrentLogger
{
    public class Logger : ILogger
    {
        private readonly int _bufferLimit;
        private readonly ILoggerTarget[] _targets;
        private readonly object _locker = new object();
        private List<LogMessage> _logInfoList;

        private volatile int _bufferId = 0;
        private volatile int _currentBufferId = 0;

        private class ThreadInfo
        {
            public List<LogMessage> Logs;
            public int ThreadId;
        }

        public Logger(int bufferLimit, ILoggerTarget[] targets)
        {
            this._bufferLimit = bufferLimit;
            this._targets = targets;
            _logInfoList = new List<LogMessage>();
        }

        public void Log(LogMessage logMessage)
        {
            if (_logInfoList.Count == _bufferLimit)
            {
                ThreadPool.QueueUserWorkItem(FlushLogsInAllTargets,
                    new ThreadInfo {Logs = _logInfoList, ThreadId = _bufferId++});
                _logInfoList = new List<LogMessage>();
            }
            _logInfoList.Add(logMessage);
        }

        private void FlushLogsInAllTargets(object objThreadInfo)
        {
            var threadInfo = (ThreadInfo) objThreadInfo;
            var logsList = threadInfo.Logs;

            lock (_locker)
            {
                while (threadInfo.ThreadId != _currentBufferId)
                    Monitor.Wait(_locker);
                foreach (ILoggerTarget currentTarget in _targets)
                    foreach (LogMessage log in logsList)
                        currentTarget.Flush(log);
                _currentBufferId++;
                Monitor.PulseAll(_locker);
            }
        }

        public void FlushRemainLogs()
        {
            ThreadPool.QueueUserWorkItem(FlushLogsInAllTargets,
                new ThreadInfo {Logs = _logInfoList, ThreadId = _bufferId++});
            lock (_locker)
            {
                while (_bufferId != _currentBufferId)
                    Monitor.Wait(_locker);
            }
        }
    }
}