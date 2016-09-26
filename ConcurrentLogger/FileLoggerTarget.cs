using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{
    class FileLoggerTarget : ILoggerTarget
    {
        private readonly FileStream _fileStream;

        public FileLoggerTarget(string filename)
        {
            _fileStream = new FileStream(filename, FileMode.Append, FileAccess.Write);
        }

        public bool Flush(LogMessage logMessage)
        {
            Write(Encoding.Default.GetBytes(logMessage.ConvertToString().ToArray()));
            _fileStream.Flush();
            return true;
        }

        public async Task<bool> FlushAsync(LogMessage logMessage)
        {
            Write(Encoding.Default.GetBytes(logMessage.ConvertToString().ToArray()));
            await _fileStream.FlushAsync();
            return true;
        }

        public void Write(byte[] log)
        {
            _fileStream.Write(log, 0, log.Length);
        }

        public void Close()
        {
            _fileStream.Close();
            _fileStream.Dispose();
        }
    }
}
