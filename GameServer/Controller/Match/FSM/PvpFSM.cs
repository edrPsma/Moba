using System;
using HFSM;
using Observable;

namespace GameServer.Controller
{
    public class PvpFSM : BaseFSM<EPvpState>
    {
        public PvpFSM(PvpRoom room, bool hasExitTime = false, EPvpState initialState = EPvpState.Confirm) : base(hasExitTime, initialState)
        {
            Room = room;
        }

        public PvpRoom Room { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            AddState(EPvpState.Confirm, new ConfirmState(this));
            AddState(EPvpState.SelectHero, new SelectHeroState(this));
            AddState(EPvpState.Loading, new LoadingState(this));
            AddState(EPvpState.Fight, new FightState(this));
            AddState(EPvpState.End, new EndState(this));
        }
    }
}