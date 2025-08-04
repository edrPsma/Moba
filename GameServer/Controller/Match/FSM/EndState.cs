using System;

namespace GameServer.Controller
{
    public class EndState : BasePvpState
    {
        public EndState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }

        protected override void OnEnter()
        {
            base.OnEnter();
            FSM.Room.IsComplete = true;
        }
    }
}