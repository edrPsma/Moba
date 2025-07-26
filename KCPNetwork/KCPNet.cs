using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace KCPNetwork
{
    public class KCPNet<TSession> where TSession : KCPSession, new()
    {
        public IPEndPoint RemotePoint { get; protected set; }

        protected UdpClient _udp;
        protected CancellationTokenSource _tokenSource;
        protected CancellationToken _token;

        public KCPNet()
        {
            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
        }

        protected void SendUDPMsg(byte[] bytes, IPEndPoint remotePoint)
        {
            if (_udp != null)
            {
                _udp.SendAsync(bytes, bytes.Length, remotePoint);
            }
        }
    }
}