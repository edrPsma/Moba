using System;

namespace GameServer.Controller
{
    public interface IController
    {
        void Initialize();
    }

    public abstract class AbstractController : IController
    {
        void IController.Initialize()
        {
            OnInitialize();
        }

        protected virtual void OnInitialize() { }
    }
}