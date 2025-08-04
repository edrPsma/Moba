using System;
using GameServer.Common;

namespace GameServer.Controller
{
    public interface IController : IGameLoop
    {

    }

    public abstract class AbstractController : IController
    {
        void IGameLoop.Initialize()
        {
            OnInitialize();
        }

        void IGameLoop.Update()
        {
            OnUpdate();
        }

        protected virtual void OnInitialize() { }

        protected virtual void OnUpdate() { }
    }
}