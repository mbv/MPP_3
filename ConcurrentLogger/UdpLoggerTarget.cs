using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ConcurrentLogger
{
    public class UdpLoggerTarget:ILoggerTarget
    {
        private readonly string _clientIp;
        private readonly string _serverIp;
        private readonly int _clientPort;
        private readonly int _serverPort;
        private UdpClient _udpClient;

        public UdpLoggerTarget(string serverIp, int serverPort, string clientIp, int clientPort)
        {
            this._serverIp = serverIp;
            this._serverPort = serverPort;
            this._clientIp = clientIp;
            this._clientPort = clientPort;
        }

        public bool Flush(LogMessage logMessage)
        {
            Write(Encoding.Default.GetBytes(logMessage.ConvertToString().ToArray()));
            _udpClient.Close();
            return true;
        }

        public async Task<bool> FlushAsync(LogMessage logInfo)
        {
            return true;
        }

        public void Write(byte[] log)
        {
            try
            {
                _udpClient = new UdpClient(new IPEndPoint(IPAddress.Parse(_clientIp), _clientPort));
                _udpClient.Send(log, log.Length, new IPEndPoint(IPAddress.Parse(_serverIp), _serverPort));
            }
            finally
            {
                _udpClient.Close();
            }
        }
    }
}
