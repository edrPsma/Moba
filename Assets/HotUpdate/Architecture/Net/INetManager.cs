using System;
using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

public interface INetManager
{
    void InitNet();

    void Connect(Action<bool> callBack);

    void Enqueue(IMessage message);

    void SendMsg(IMessage message);

    void Register<T>(Action<T> onMsgReceive);
}
