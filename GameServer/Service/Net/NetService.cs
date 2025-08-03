using System;
using System.Collections.Generic;
using GameServer.Common;
using Google.Protobuf;
using KCPNetwork;

namespace GameServer.Service
{
    public interface INetService
    {
        void Enqueue(ServerSession session, IMessage message);

        void StartServer();
    }

    [Reflection(typeof(INetService))]
    public class NetService : AbstractService, INetService
    {
        public static readonly string QueLock = "QueLock";
        private KCPServer<ServerSession> _server = new KCPServer<ServerSession>();
        private Queue<MessagePackage> _queue = new Queue<MessagePackage>();

        public NetService()
        {
            KCPTool.Log = str => Debug.Log(str);
            KCPTool.Warn = str => Debug.Warn(str);
            KCPTool.Error = str => Debug.Error(str);
        }

        public void StartServer()
        {
            _server.StartAsServer(ServerConfig.ServerIP, ServerConfig.ServerPort);
            Debug.ColorLog(LogColor.Green, "服务器启动");
        }

        public void Enqueue(ServerSession session, IMessage message)
        {
            lock (QueLock)
            {
                _queue.Enqueue(new MessagePackage(session, message));
            }
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_queue.Count > 0)
            {
                lock (QueLock)
                {
                    MessagePackage messagePackage = _queue.Dequeue();
                    HandOutMessage(messagePackage);
                }
            }
        }

        private void HandOutMessage(MessagePackage package)
        {

        }
    }

    public class MessagePackage
    {
        public ServerSession Session;
        public IMessage Message;

        public MessagePackage(ServerSession session, IMessage message)
        {
            Session = session;
            Message = message;
        }
    }
}