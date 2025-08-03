using System;
using System.Buffers;
using System.Net;
using System.Net.Sockets.Kcp;
using System.Linq;
using Google.Protobuf;
using System.Threading;
using System.Threading.Tasks;

namespace KCPNetwork
{
    public abstract class KCPSession : IKcpCallback
    {
        public uint SessionID { get; private set; }
        public Kcp Kcp { get; private set; }
        public ESessionState State { get; private set; } = ESessionState.None;
        public Action<uint> OnSessionClose;

        IPEndPoint _remotePoint;
        Action<byte[], IPEndPoint> _udpSender;
        CancellationTokenSource _tokenSource;
        CancellationToken _token;

        public void Initialize(uint sessionID, IPEndPoint remotePoint, Action<byte[], IPEndPoint> udpSender)
        {
            SessionID = sessionID;
            Kcp = new Kcp(sessionID, this);
            State = ESessionState.Connected;
            _remotePoint = remotePoint;
            _udpSender = udpSender;

            Kcp.NoDelay(1, 10, 2, 1);
            Kcp.WndSize(64, 64);
            Kcp.SetMtu(512);

            OnConnected();

            _tokenSource = new CancellationTokenSource();
            _token = _tokenSource.Token;
            Task.Run(Update, _token);
        }

        public void Output(IMemoryOwner<byte> buffer, int avalidLength)
        {
            byte[] bytes = buffer.Memory.ToArray();
            _udpSender?.Invoke(bytes, _remotePoint);
        }

        public void Send(byte[] msg)
        {
            if (IsConnected())
            {
                Kcp.Send(msg.AsSpan());
            }
            else
            {
                KCPTool.Warn?.Invoke("Session Disconnected.Can not send msg.");
            }
        }

        public void Send(IMessage msg)
        {
            if (IsConnected())
            {
                byte[] bytes = Serialize(msg);
                if (bytes != null)
                {
                    Kcp.Send(bytes.AsSpan());
                }
            }
            else
            {
                KCPTool.Warn?.Invoke("Session Disconnected.Can not send msg.");
            }
        }

        public void Recive(byte[] buffer)
        {
            Kcp.Input(buffer.AsSpan());
        }

        async void Update()
        {
            try
            {
                while (true)
                {
                    DateTime now = DateTime.UtcNow;
                    OnUpdate(now);
                    if (_token.IsCancellationRequested)
                    {
                        KCPTool.Log?.Invoke("SessionUpdate Task is Cancelled.");
                        break;
                    }
                    else
                    {
                        Kcp.Update(now);
                        int len;
                        while ((len = Kcp.PeekSize()) > 0)
                        {
                            var buffer = new byte[len];
                            if (Kcp.Recv(buffer) >= 0)
                            {
                                IMessage message = DeSerialize(buffer);
                                OnReciveMsg(message);
                                Recive(buffer);
                            }
                        }
                        await Task.Delay(10);
                    }
                }
            }
            catch (Exception e)
            {
                KCPTool.Warn?.Invoke($"Session Update Exception: {e}");
            }
        }

        public void CloseSession()
        {
            _tokenSource.Cancel();
            OnDisConnected();

            OnSessionClose?.Invoke(SessionID);
            OnSessionClose = null;

            State = ESessionState.DisConnected;
            _remotePoint = null;
            _udpSender = null;
            SessionID = 0;

            Kcp = null;
            _tokenSource = null;
        }

        public override bool Equals(object obj)
        {
            if (obj is KCPSession)
            {
                KCPSession us = obj as KCPSession;
                return SessionID == us.SessionID;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return SessionID.GetHashCode();
        }
        public uint GetSessionID()
        {
            return SessionID;
        }

        public bool IsConnected()
        {
            return State == ESessionState.Connected;
        }

        protected virtual void OnUpdate(DateTime now) { }
        protected virtual void OnConnected() { }
        protected virtual void OnReciveMsg(IMessage msg) { }
        protected virtual void OnDisConnected() { }

        public abstract byte[] Serialize<T>(T msg) where T : IMessage;
        public abstract IMessage DeSerialize(byte[] bytes);
    }

    public enum ESessionState : byte
    {
        None,

        Connected,

        DisConnected
    }
}