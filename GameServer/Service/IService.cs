using System;
using GameServer.Common;

namespace GameServer.Service
{
    public interface IService : IGameLoop
    {

    }

    public abstract class AbstractService : IService
    {
        void IGameLoop.Update()
        {
            OnUpdate();
        }

        void IGameLoop.Initialize()
        {
            OnInitialize();
        }

        protected virtual void OnUpdate()
        {

        }

        protected virtual void OnInitialize()
        {

        }
    }
}
