using System;

namespace GameServer.Controller
{
    public class FightState : BasePvpState
    {
        public FightState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }
    }
}