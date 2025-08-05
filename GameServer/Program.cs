using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameServer.Common;
using GameServer.Controller;
using GameServer.Service;
using GameServer.Test;

namespace GameServer

{
    class Program
    {
        static void Main(string[] args)
        {
            Debug.InitSettings();
            ReflectionManager.Instance.Init();

            Builder.Get<INetService>().StartServer();

            List<IGameLoop> gameLoops = new List<IGameLoop>();

            Builder.ForEach(item =>
            {
                if (item is IGameLoop)
                {
                    IGameLoop looper = item as IGameLoop;
                    gameLoops.Add(looper);
                    looper.Initialize();
                }
            });

            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

            Task.Run(() =>
            {
                while (true)
                {
                    if (cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    try
                    {
                        foreach (var item in gameLoops)
                        {
                            item.Update();
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.Error(e);
                    }
                    Thread.Sleep(10);
                }
            }, cancellationTokenSource.Token);

            while (Console.ReadLine() != "quit")
            {
                Thread.Sleep(10);
            }
            cancellationTokenSource.Cancel();

            Builder.Get<INetService>().Close();
            Console.ReadKey();
        }
    }
}