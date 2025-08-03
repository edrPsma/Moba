using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace KCPNetwork
{
    public class KCPClient<TSession> : KCPNet<TSession> where TSession : KCPSession, new()
    {
        public TSession Session { get; private set; }

        public void StartAsClient(string ip, int port)
        {
            _udp = new UdpClient(0);
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                _udp.Client.IOControl((IOControlCode)(-1744830452), new byte[] { 0, 0, 0, 0 }, null);
            }
            RemotePoint = new IPEndPoint(IPAddress.Parse(ip), port);
            KCPTool.Log?.Invoke("Client Start...");

            Task.Run(ClientRecive, _token);
        }

        public void CloseClient()
        {
            if (Session != null)
            {
                Session.CloseSession();
            }
        }

        public Task<bool> ConnectServer(int interval, int maxintervalSum = 5000)
        {
            SendUDPMsg(new byte[4], RemotePoint);
            int checkTimes = 0;
            Task<bool> task = Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);
                    checkTimes += interval;
                    if (Session != null && Session.IsConnected())
                    {
                        return true;
                    }
                    else
                    {
                        if (checkTimes > maxintervalSum)
                        {
                            return false;
                        }
                    }
                }
            });
            return task;
        }

        async void ClientRecive()
        {
            UdpReceiveResult result;
            while (true)
            {
                try
                {
                    if (_token.IsCancellationRequested)
                    {
                        KCPTool.Log?.Invoke("ClientRecive Task is Cancelled.");
                        break;
                    }
                    result = await _udp.ReceiveAsync();

                    if (Equals(RemotePoint, result.RemoteEndPoint))
                    {
                        uint sid = BitConverter.ToUInt32(result.Buffer, 0);
                        if (sid == 0)
                        {
                            //sid 数据
                            if (Session != null && Session.IsConnected())
                            {
                                //已经建立连接，初始化完成了，收到了多的sid,直接丢弃。
                                KCPTool.Warn?.Invoke("Client is Init Done,Sid Surplus.");
                            }
                            else
                            {
                                //未初始化，收到服务器分配的sid数据，初始化一个客户端session
                                sid = BitConverter.ToUInt32(result.Buffer, 4);
                                KCPTool.Log?.Invoke($"UDP Request Conv Sid: {sid}");

                                //会话处理
                                Session = new TSession();
                                Session.Initialize(sid, RemotePoint, SendUDPMsg);
                                Session.OnSessionClose = OnClientSessionClose;
                            }
                        }
                        else
                        {
                            //处理业务逻辑
                            if (Session != null && Session.IsConnected())
                            {
                                Session.Recive(result.Buffer);
                            }
                            else
                            {
                                //没初始化且sid!=0，数据消息提前到了，直接丢弃消息，直到初
                                //始化完成，kcp重传再开始处理。
                                KCPTool.Warn?.Invoke("Client is Initing...");
                            }
                        }
                    }
                    else
                    {
                        KCPTool.Warn?.Invoke("Client Udp Recive illegal target Data.");
                    }
                }
                catch (Exception e)
                {
                    KCPTool.Warn?.Invoke($"Client Udp Recive Data Exception:{e}");
                }
            }
        }

        private void OnClientSessionClose(uint sessionID)
        {
            _tokenSource.Cancel();
            if (_udp != null)
            {
                _udp.Close();
                _udp = null;
            }
            Session = null;
            KCPTool.Warn?.Invoke($"Client Session Close,sid: {sessionID}");
        }
    }
}