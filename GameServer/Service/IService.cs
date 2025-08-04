using System;

namespace GameServer.Service
{
    public interface IService
    {
        void Initialize();

        void Update();
    }

    public abstract class AbstractService : IService
    {
        void IService.Update()
        {
            OnUpdate();
        }

        void IService.Initialize()
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
