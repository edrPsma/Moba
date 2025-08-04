using System;
using System.Collections.Generic;
using GameServer.Common;
using Google.Protobuf;
using KCPNetwork;
using Observable;
using Protocol;

namespace GameServer.Service
{
    public interface INetService
    {
        void Enqueue(ServerSession session, IMessage message);

        void StartServer();

        void Close();

        void Register<T>(Action<ServerSession, T> onMsgReceive);
    }

    [Reflection(typeof(INetService))]
    public class NetService : AbstractService, INetService
    {
        public static readonly string QueLock = "QueLock";
        private KCPServer<ServerSession> _server = new KCPServer<ServerSession>();
        private Queue<MessagePackage> _queue = new Queue<MessagePackage>();
        TypeEventSource<short> _eventSource;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            KCPTool.Log = str => Debug.Log(str);
            KCPTool.Warn = str => Debug.Warn(str);
            KCPTool.Error = str => Debug.Error(str);
            _eventSource = new TypeEventSource<short>();
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

            if (_server != null)
            {
                if (_queue.Count > 0)
                {
                    lock (QueLock)
                    {
                        MessagePackage messagePackage = _queue.Dequeue();
                        HandOutMessage(messagePackage);
                    }
                }
            }
        }

        private void HandOutMessage(MessagePackage package)
        {
            short msgID = MessageBuilder.QueryMessageID(package.Message.GetType());
            _eventSource.Trigger(msgID, package);
        }

        public void Close()
        {
            _server.CloseServer();
            _server = null;
        }

        public void Register<T>(Action<ServerSession, T> onMsgReceive)
        {
            short msgID = MessageBuilder.QueryMessageID(typeof(T));
            _eventSource.Register<MessagePackage>(msgID, package => onMsgReceive?.Invoke(package.Session, (T)package.Message));
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