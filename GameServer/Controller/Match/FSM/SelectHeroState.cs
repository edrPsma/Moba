using System;

namespace GameServer.Controller
{
    public class SelectHeroState : BasePvpState
    {
        public SelectHeroState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }
    }
}