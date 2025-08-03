using System.Collections;
using System.Collections.Generic;
using Google.Protobuf;
using UnityEngine;

public interface INetManager
{
    void Connect();

    void Enqueue(IMessage message);
}
