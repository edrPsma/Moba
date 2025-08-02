using System;
using GameServer.Common;
using GameServer.Test;

namespace GameServer

{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
            ReflectionManager.Instance.Init();

            TestUI testUI = Builder.NewAndInject<TestUI>();
            Console.WriteLine(testUI.TestController);
            testUI.Log();

            Console.ReadKey();
        }
    }
}