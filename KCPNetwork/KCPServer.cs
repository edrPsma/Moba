using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Google.Protobuf;

namespace KCPNetwork
{
    public class KCPServer<TSession> : KCPNet<TSession> where TSession : KCPSession, new()
    {
        protected Dictionary<uint, TSession> _sessionDic = null;
        TSession _cacheSession;
        private uint _curSessionid = 0;

        public void BroadCastMsg<T>(T msg) where T : IMessage, new()
        {
            byte[] bytes = _cacheSession.Serialize(msg);
            foreach (var item in _sessionDic)
            {
                item.Value.Send(bytes);
            }
        }

        public void StartAsServer(string ip, int port)
        {
            _sessionDic = new Dictionary<uint, TSession>();
            _cacheSession = new TSession();

            _udp = new UdpClient(new IPEndPoint(IPAddress.Parse(ip), port));
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _udp.Client.IOControl((IOControlCode)(-1744830452), new byte[] { 0, 0, 0, 0 }, null);
            }
            RemotePoint = new IPEndPoint(IPAddress.Parse(ip), port);
            KCPTool.Log?.Invoke("Server Start...");
            Task.Run(ServerRecive, _token);
        }

        async void ServerRecive()
        {
            UdpReceiveResult result;
            while (true)
            {
                try
                {
                    if (_token.IsCancellationRequested)
                    {
                        KCPTool.Log?.Invoke("SeverRecive Task is Cancelled.");
                        break;
                    }
                    result = await _udp.ReceiveAsync();
                    uint sid = BitConverter.ToUInt32(result.Buffer, 0);
                    if (sid == 0)
                    {
                        sid = GenerateUniqueSessionID();
                        byte[] sid_bytes = BitConverter.GetBytes(sid);
                        byte[] conv_bytes = new byte[8];
                        Array.Copy(sid_bytes, 0, conv_bytes, 4, 4);
                        SendUDPMsg(conv_bytes, result.RemoteEndPoint);
                    }
                    else
                    {
                        if (!_sessionDic.TryGetValue(sid, out TSession session))
                        {
                            session = new TSession();
                            session.Initialize(sid, result.RemoteEndPoint, SendUDPMsg);
                            session.OnSessionClose = OnServerSessionClose;
                            lock (_sessionDic)
                            {
                                _sessionDic.Add(sid, session);
                            }
                        }
                        else
                        {
                            session = _sessionDic[sid];
                        }
                        session.Recive(result.Buffer);
                    }
                }
                catch (Exception e)
                {
                    KCPTool.Warn?.Invoke($"Server Udp Recive Data Exception:{e}");
                }
            }
        }
        void OnServerSessionClose(uint sid)
        {
            if (_sessionDic.ContainsKey(sid))
            {
                lock (_sessionDic)
                {
                    _sessionDic.Remove(sid);
                    KCPTool.Warn?.Invoke($"Session:{sid} remove from _sessionDic.");
                }
            }
            else
            {
                KCPTool.Error?.Invoke($"Session:{sid} cannot find in _sessionDic");
            }
        }

        public void CloseServer()
        {
            foreach (var item in _sessionDic)
            {
                item.Value.CloseSession();
            }
            _sessionDic = null;

            if (_udp != null)
            {
                _udp.Close();
                _udp = null;
                _tokenSource.Cancel();
            }
        }

        public uint GenerateUniqueSessionID()
        {
            lock (_sessionDic)
            {
                while (true)
                {
                    ++_curSessionid;
                    if (_curSessionid == uint.MaxValue)
                    {
                        _curSessionid = 1;
                    }
                    if (!_sessionDic.ContainsKey(_curSessionid))
                    {
                        break;
                    }
                }
            }
            return _curSessionid;
        }
    }
}