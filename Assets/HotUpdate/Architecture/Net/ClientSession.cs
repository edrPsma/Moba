using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Google.Protobuf;
using KCPNetwork;
using Protocol;
using UnityEngine;

public class ClientSession : KCPSession
{
    protected override void OnConnected()
    {
        Debug.Log("Thread:" + Thread.CurrentThread.ManagedThreadId + " Connected.");
    }

    protected override void OnDisConnected()
    {
        Debug.Log("Thread:" + Thread.CurrentThread.ManagedThreadId + " DisConnected.");
    }

    protected override void OnReciveMsg(IMessage msg)
    {
        Debug.Log(string.Format("Thread:{0} Sid;{1},RcvClient:{2}", Thread.CurrentThread.ManagedThreadId, SessionID, msg));
        if (msg is Protocol.Ping)
        {
            Protocol.Ping ping = (Protocol.Ping)msg;
            if (ping.IsOver)
            {
                CloseSession();
            }
            else
            {
                checkCounter = 0;
                int delay = (int)DateTime.UtcNow.Subtract(sendTime).TotalMilliseconds;
                Debug.Log(string.Format("Thread:{0} NetDelay:{1}", Thread.CurrentThread.ManagedThreadId, delay));
            }
        }
        else if (msg is Test.TestInfo)
        {
            Test.TestInfo testInfo = (Test.TestInfo)msg;
            Debug.Log(testInfo.Name);
        }
    }

    private DateTime sendTime;
    private int checkCounter;
    private DateTime checkTime = DateTime.UtcNow.AddSeconds(5);
    protected override void OnUpdate(DateTime now)
    {
        if (now > checkTime)
        {
            sendTime = now;
            checkTime = now.AddSeconds(5);
            checkCounter++;
            if (checkCounter > 3)
            {
                Protocol.Ping pingMsg = new Protocol.Ping { IsOver = true };
                OnReciveMsg(pingMsg);
            }
            else
            {
                Protocol.Ping pingMsg = new Protocol.Ping { IsOver = false };
                Send(pingMsg);
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

        Debug.Log($"{type.Name} {msgId}");

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
        Debug.Log($"收到消息,ID: {messageID}");

        IMessage message = MessageBuilder.Build(messageID);

        return message.Descriptor.Parser.ParseFrom(bytes, 2, bytes.Length - 2);
    }
}
