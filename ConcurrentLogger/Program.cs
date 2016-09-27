using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{
    class Program
    {
        static void Main(string[] args)
        {
            int bufferLimit = 3;

            ILoggerTarget[] logTarget = new ILoggerTarget[]
                {new FileLoggerTarget("_Log.txt"), new UdpLoggerTarget("127.0.0.1", 9000, "127.0.0.1", 10000)};

            var factory = new LogsFactory(new Logger(bufferLimit, logTarget));
            factory.Create(10000, LogLevel.Info);

            Console.ReadLine();
        }
    }
}