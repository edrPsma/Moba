using System;
using GameServer.Common;
using Google.Protobuf;
using Protocol;

namespace GameServer.Service
{
    public class ServerSession : KCPNetwork.KCPSession
    {
        public INetService NetService => Builder.Get<INetService>();
        public ICacheService CacheService => Builder.Get<ICacheService>();
        public uint UId;

        protected override void OnConnected()
        {
            Console.WriteLine($"Client Online,Sid:{SessionID}");
        }

        protected override void OnDisConnected()
        {
            Console.WriteLine($"Client Offline,Sid: {SessionID},UId: {UId}");
            CacheService.Offline(UId);
            UId = 0;
        }

        protected override void OnReciveMsg(IMessage msg)
        {
            if (msg is Ping)
            {
                Ping ping = (Ping)msg;
                if (ping.IsOver)
                {
                    CloseSession();
                }
                else
                {
                    //收到ping请求，则重置检查计数，并回复ping消息到客户端
                    checkCounter = 0;
                    Ping pingMsg = new Ping
                    {
                        IsOver = false
                    };
                    Send(pingMsg);
                }
            }
            else
            {
                NetService.Enqueue(this, msg);
            }
        }

        private int checkCounter;
        DateTime checkTime = DateTime.UtcNow.AddSeconds(5);
        protected override void OnUpdate(DateTime now)
        {
            if (now > checkTime)
            {
                checkTime = now.AddSeconds(5);
                checkCounter++;
                if (checkCounter > 3)
                {
                    Ping pingMsg = new Ping
                    {
                        IsOver = true
                    };
                    OnReciveMsg(pingMsg);
                }
            }
        }

        public override byte[] Serialize<T>(T msg)
        {
            int size = msg.CalculateSize();
            byte[] bytes = new byte[size + 2];
            // 前两位写入消息ID
            Type type = msg.GetType();
            short msgId = MessageBuilder.QueryMessageID(type);

            using (CodedOutputStream codedOutputStream = new CodedOutputStream(bytes))
            {
                codedOutputStream.WriteRawTag((byte)((msgId >> 8) & 0xFF));
                codedOutputStream.WriteRawTag((byte)(msgId & 0xFF));
                msg.WriteTo(codedOutputStream);
                codedOutputStream.CheckNoSpaceLeft();
            }

            return bytes;
        }

        public override IMessage DeSerialize(byte[] bytes)
        {
            short messageID = (short)((bytes[0] << 8) | bytes[1]);

            IMessage message = MessageBuilder.Build(messageID);
            return message.Descriptor.Parser.ParseFrom(bytes, 2, bytes.Length - 2);
        }
    }

    public static class ServerSessionExtra
    {
        public static byte[] Serialize<T>(this T msg) where T : IMessage
        {
            int size = msg.CalculateSize();
            byte[] bytes = new byte[size + 2];
            // 前两位写入消息ID
            Type type = msg.GetType();
            short msgId = MessageBuilder.QueryMessageID(type);

            using (CodedOutputStream codedOutputStream = new CodedOutputStream(bytes))
            {
                codedOutputStream.WriteRawTag((byte)((msgId >> 8) & 0xFF));
                codedOutputStream.WriteRawTag((byte)(msgId & 0xFF));
                msg.WriteTo(codedOutputStream);
                codedOutputStream.CheckNoSpaceLeft();
            }

            return bytes;
        }
    }
}