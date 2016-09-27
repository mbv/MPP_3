using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ConcurrentLogger;

namespace ConcurrentLogger_UnitTests
{
    [TestClass]
    public class LoggerTest
    {
        [TestMethod]
        public void TestOneTarget()
        {
            int bufferLimit = 1,
                logsCount = 10000;

            var fakeTargetStream = new FakeTargetStream();
            StringBuilder stringBuilder = new StringBuilder();

            var factory = new LogsFactory(new Logger(bufferLimit, new ILoggerTarget[] {fakeTargetStream}));
            factory.Create(logsCount, LogLevel.Info);

            for (int i = 0; i < logsCount; i++)
                stringBuilder.Append(i);

            CollectionAssert.AreEqual(Encoding.Default.GetBytes(stringBuilder.ToString()), fakeTargetStream.GetMessage());
        }

        [TestMethod]
        public void TestTwoTargets()
        {
            int bufferLimit = 5,
                logsCount = 10000;
            var firstFakeTargetStream = new FakeTargetStream();
            var secondFakeTargetStream = new FakeTargetStream();
            StringBuilder stringBuilder = new StringBuilder();

            var factory =
                new LogsFactory(new Logger(bufferLimit,
                    new ILoggerTarget[] {firstFakeTargetStream, secondFakeTargetStream}));
            factory.Create(logsCount, LogLevel.Debug);

            for (int i = 0; i < logsCount; i++)
                stringBuilder.Append(i);
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(stringBuilder.ToString()),
                firstFakeTargetStream.GetMessage());
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(stringBuilder.ToString()),
                secondFakeTargetStream.GetMessage());
        }

        [TestMethod]
        public void TestUdpTarget()
        {
            int bufferLimit = 5,
                logsCount = 100;
            var udpServer = new TestUdpServer("127.0.0.1", 9000);
            var targetUdp = new UdpLoggerTarget("127.0.0.1", 9000, "127.0.0.1", 10000);
            StringBuilder stringBuilder = new StringBuilder();
            udpServer.StartReceive();
            var logsCreator = new LogsFactory(new Logger(bufferLimit, new ILoggerTarget[] {targetUdp}));
            logsCreator.Create(logsCount, LogLevel.Debug);
            for (int i = 0; i < logsCount; i++)
                stringBuilder.Append(i);
            udpServer.Synchronize();
            udpServer.Close();
            CollectionAssert.AreEqual(Encoding.Default.GetBytes(stringBuilder.ToString()), udpServer.GetMessage());
        }
    }
}