using System;

namespace GameServer.Service
{
    public interface IService
    {
        void Update();
    }

    public abstract class AbstractService : IService
    {
        void IService.Update()
        {
            OnUpdate();
        }

        protected virtual void OnUpdate()
        {

        }
    }
}
