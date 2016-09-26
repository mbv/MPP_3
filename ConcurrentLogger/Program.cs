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

            ILoggerTarget[] logTarget = new ILoggerTarget[] {new FileLoggerTarget("_Log.txt")};

            var factory = new LogsFactory(new Logger(bufferLimit, logTarget));
            factory.Create(10000, LogLevel.Info);

            Console.ReadLine();
        }
    }
}