using System;

namespace GameServer.Controller
{
    public class LoadingState : BasePvpState
    {
        public LoadingState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }
    }
}