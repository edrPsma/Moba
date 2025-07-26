using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Google.Protobuf;
using KCPNetwork;
using Protocol;
using Sirenix.OdinInspector;
using Test;
using UnityEngine;

public class Test1 : MonoBehaviour
{

    Client client;

    void Start()
    {
        KCPTool.Log = Debug.Log;
        KCPTool.Warn = Debug.LogWarning;
        KCPTool.Error = Debug.LogError;

        string ip = "127.0.0.1";
        client = new Client();
        client.StartAsClient(ip, 17666);
        client.ConnectServer(200, 500);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            client.CloseClient();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            client.Session.Send(new TestInfo
            {
                Name = "msg from unity:www.qiqiker.com"
            });
        }
    }

    void OnApplicationQuit()
    {
        client.CloseClient();
    }

    class Client : KCPClient<ClientSession>
    {

    }
}
