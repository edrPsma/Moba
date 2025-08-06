using System;
using GameServer.Common;
using GameServer.Service;

namespace GameServer.Controller
{
    public class EndState : BasePvpState
    {
        [Inject] public ICacheService CacheService;

        public EndState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }

        protected override void OnEnter()
        {
            base.OnEnter();
            FSM.Room.IsComplete = true;
            for (int i = 0; i < FSM.Room.Players.Length; i++)
            {
                CacheService.SetRoom(FSM.Room.Players[i], 0);
            }
            Debug.ColorLog(LogColor.Blue, $"战斗结束,Room: {FSM.Room.RoomID}");
        }
    }
}