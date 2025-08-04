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
            Console.WriteLine("Hello, World!");
            Debug.InitSettings();
            ReflectionManager.Instance.Init();

            Builder.Get<INetService>().StartServer();

            List<IService> services = new List<IService>();
            List<IController> controllers = new List<IController>();

            Builder.ForEach(item =>
            {
                if (item is IService)
                {
                    IService service = item as IService;
                    services.Add(service);
                    service.Initialize();
                }
                else if (item is IController)
                {
                    IController controller = item as IController;
                    controller.Initialize();
                    controllers.Add(controller);
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
                    foreach (var item in services)
                    {
                        item.Update();
                    }
                    Thread.Sleep(10);
                }
            }, cancellationTokenSource.Token);

            while (Console.ReadLine() != "quit")
            {
                Thread.Sleep(10);
            }
            cancellationTokenSource.Cancel();

            foreach (var item in controllers)
            {
                item.ShutDown();
            }

            Builder.Get<INetService>().Close();
            Console.ReadKey();
        }
    }
}