using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Protobuf;
using KCPNetwork;
using Template;
using UnityEngine;

public class NetManager : MonoSingleton<INetManager, NetManager>, INetManager
{
    static readonly string QueLock = "QueLock";
    KCPClient<ClientSession> _client = new KCPClient<ClientSession>();
    Queue<IMessage> _queue = new Queue<IMessage>();
    Task<bool> _checkTask;
    int _connectCount;

    protected override void OnInit()
    {
        base.OnInit();
        KCPTool.Log = str => Debug.Log(str);
        KCPTool.Warn = str => Debug.LogWarning(str);
        KCPTool.Error = str => Debug.LogError(str);
        GameObject.DontDestroyOnLoad(gameObject);
        _client.StartAsClient("127.0.0.1", 10777);
    }

    public void Connect()
    {
        _connectCount = 0;
        _checkTask = _client.ConnectServer(100);
    }

    public void Enqueue(IMessage message)
    {
        lock (QueLock)
        {
            _queue.Enqueue(message);
        }
    }

    void Update()
    {
        if (_checkTask != null)
        {
            if (_checkTask.IsCompleted)
            {
                if (_checkTask.Result)
                {
                    TipsForm.ShowTips("连接服务器成功!");
                    _checkTask = null;
                }
                else
                {
                    _connectCount++;
                    if (_connectCount > 4)
                    {
                        TipsForm.ShowTips("无法连接服务器,请检查你的网络");
                        _checkTask = null;
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

    }
}
