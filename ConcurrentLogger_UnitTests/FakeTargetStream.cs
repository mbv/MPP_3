using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConcurrentLogger;

namespace ConcurrentLogger_UnitTests
{ 
        public class FakeTargetStream : ILoggerTarget
    {
        private readonly MemoryStream _currentStream;
        private readonly StringBuilder _message = new StringBuilder();

        public FakeTargetStream()
        {
            _currentStream = new MemoryStream();
        }

        public void Write(LogMessage logMessage)
        {
            _message.Append(logMessage.Message.Substring(5));
            byte[] log = Encoding.Default.GetBytes(logMessage.ConvertToString().ToArray());
            _currentStream.Write(log, 0, log.Length);
        }

        public bool Flush(LogMessage logMessage)
        {
            Write(logMessage);
            _currentStream.Flush();
            return true;
        }

        public async Task<bool> FlushAsync(LogMessage logMessage)
        {
            Write(logMessage);
            await _currentStream.FlushAsync();
            return true;
        }

        public byte[] GetMessage()
        {
            return Encoding.Default.GetBytes(_message.ToString());
        }
    }
  
}
