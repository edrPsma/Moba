using System;

namespace GameServer.Common
{
    public interface IGameLoop
    {
        void Initialize();

        void Update();
    }
}