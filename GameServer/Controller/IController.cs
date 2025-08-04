using System;

namespace GameServer.Controller
{
    public interface IController
    {
        void Initialize();

        void ShutDown();
    }

    public abstract class AbstractController : IController
    {
        void IController.Initialize()
        {
            OnInitialize();
        }

        void IController.ShutDown()
        {
            OnShutDown();
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnShutDown() { }
    }
}