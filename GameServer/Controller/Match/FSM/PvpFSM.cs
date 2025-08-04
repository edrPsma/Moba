using System;
using HFSM;

namespace GameServer.Controller
{
    public class PvpFSM : BaseFSM<EPvpState>
    {
        public PvpFSM(bool hasExitTime = false, EPvpState initialState = EPvpState.Confirm) : base(hasExitTime, initialState) { }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            AddState(EPvpState.Confirm, new ConfirmState());
            AddState(EPvpState.SelectHero, new SelectHeroState());
            AddState(EPvpState.Loading, new LoadingState());
            AddState(EPvpState.Fight, new FightState());
            AddState(EPvpState.End, new EndState());
        }
    }
}