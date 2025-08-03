using System;
using GameServer.Common;
using GameServer.Service;
using GameServer.Test;

namespace GameServer

{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            Debug.InitSettings();
            ReflectionManager.Instance.Init();

            Builder.Get<INetService>().StartServer();
            Console.ReadKey();
        }
    }
}