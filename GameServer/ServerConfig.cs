using System;

namespace GameServer
{
    public class ServerConfig
    {
        public const string ServerIP = "192.168.0.110";
        public const int ServerPort = 17666;

        public const int ConfirmCountDown = 15;
        public const int SelectHeroCountDown = 32;
        public const int FightCountDown = 600;
        public const int LogicFrameInterval = 66;
        public const int ChaseFrameCount = 20;
    }
}