using System;
using GameServer.Common;

namespace GameServer.Test
{
    public interface ITestController { }

    [Reflection(typeof(ITestController))]
    public class TestController : ITestController
    {

    }

    public class TestUI
    {
        [Inject] public ITestController TestController;
        [Inject] ITestController TestController2;

        public void Log()
        {
            Console.WriteLine(TestController2);
        }
    }
}
