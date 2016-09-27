using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger_UnitTests
{
    class TestUdpServer
    {
        private UdpClient _udpClient;
        private readonly string _serverIp;
        private readonly int _serverPort;
        private readonly StringBuilder _stringBuilder;
        private volatile bool _isReadSocket;
        private Task _receiveAsync;

        public TestUdpServer(string serverIp, int serverPort)
        {
            _stringBuilder = new StringBuilder();
            this._serverIp = serverIp;
            this._serverPort = serverPort;
        }

        public void StartReceive()
        {
            _isReadSocket = true;
            _udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort));
            _receiveAsync = Task.Factory.StartNew(TaskReceive);
        }

        public void TaskReceive()
        {
            try
            {
                while (_isReadSocket)
                {
                    IPEndPoint clientPoint = null;
                    var receiveBytes = _udpClient.Receive(ref clientPoint);
                    string message = Encoding.Default.GetString(receiveBytes);
                    int index = message.IndexOf("task", StringComparison.Ordinal) + 5;
                    _stringBuilder.Append(message.Substring(index, message.Length - index - 2));
                }
            }
            finally
            {
                _udpClient.Close();
            }
        }

        public void Synchronize()
        {
            _receiveAsync.Wait(1500);
            _isReadSocket = false;
            _receiveAsync = null;
        }

        public byte[] GetMessage()
        {
            return Encoding.Default.GetBytes(_stringBuilder.ToString());
        }

        public void Close()
        {
            _udpClient.Close();
        }
    }
}
