using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{
    public interface ILoggerTarget
    {
        bool Flush(LogMessage logMessage);
        Task<bool> FlushAsync(LogMessage logInfo);
    }
}
