using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf;
using KCPNetwork;
using Observable;
using Template;
using UnityEngine;
using Protocol;

public class NetManager : MonoSingleton<INetManager, NetManager>, INetManager
{
    static readonly string QueLock = "QueLock";
    KCPClient<ClientSession> _client;
    Queue<IMessage> _queue;
    Task<bool> _checkTask;
    int _connectCount;
    Action<bool> _connectCallBack;
    TypeEventSource<short> _eventSource;

    protected override void OnInit()
    {
        base.OnInit();
        KCPTool.Log = str => Debug.Log(str);
        KCPTool.Warn = str => Debug.LogWarning(str);
        KCPTool.Error = str => Debug.LogError(str);

        _eventSource = new TypeEventSource<short>();
        GameObject.DontDestroyOnLoad(gameObject);
    }

    public void InitNet()
    {
        _client?.CloseClient();
        _client = new KCPClient<ClientSession>();
        _queue = new Queue<IMessage>();
        _client.StartAsClient("192.168.0.110", 17666);
    }

    public void Connect(Action<bool> callBack)
    {
        _connectCount = 0;
        _connectCallBack = callBack;
        _checkTask = _client.ConnectServer(100);
    }

    public void SendMsg(IMessage message)
    {
        if (_client != null && _client.Session != null)
        {
            _client.Session.Send(message);
        }
    }

    public void Register<T>(Action<T> onMsgReceive)
    {
        short msgID = MessageBuilder.QueryMessageID(typeof(T));
        _eventSource.Register<IMessage>(msgID, msg => onMsgReceive?.Invoke((T)msg));
    }

    public void Enqueue(IMessage message)
    {
        lock (QueLock)
        {
            _queue.Enqueue(message);
        }
    }

    void OnDestroy()
    {
        _client.CloseClient();
    }

    void Update()
    {
        if (_checkTask != null)
        {
            if (_checkTask.IsCompleted)
            {
                if (_checkTask.Result)
                {
                    _checkTask = null;
                    _connectCount = 0;
                    _connectCallBack?.Invoke(true);
                }
                else
                {
                    _connectCount++;
                    if (_connectCount > 4)
                    {
                        _checkTask = null;
                        _connectCallBack?.Invoke(false);
                    }
                    else
                    {
                        _checkTask = _client.ConnectServer(100);
                    }
                }
            }

        }

        if (_client != null && _client.Session != null)
        {
            if (_queue.Count > 0)
            {
                lock (QueLock)
                {
                    IMessage message = _queue.Dequeue();
                    HandOutMessage(message);
                }
            }
        }

    }

    private void HandOutMessage(IMessage message)
    {
        short msgID = MessageBuilder.QueryMessageID(message.GetType());
        _eventSource.Trigger(msgID, message);
    }
}
