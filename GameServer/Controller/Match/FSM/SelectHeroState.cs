using System;
using GameServer.Common;
using GameServer.Service;

namespace GameServer.Controller
{
    public class SelectHeroState : BasePvpState
    {
        [Inject] public ITimeService TimeService;
        [Inject] public IMatchController MatchController;
        int _checkTaskID;

        public SelectHeroState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }

        protected override void OnEnter()
        {
            base.OnEnter();

            InitSelectData();
            _checkTaskID = TimeService.AddTask(ServerConfig.SelectHeroCountDown * 1000, ReachTimeLimit);
            FSM.Room.EventSource.Register<EventSelectHero>(OnSelectHero);
        }

        protected override void OnExit()
        {
            base.OnExit();
            FSM.Room.EventSource.UnRegister<EventSelectHero>(OnSelectHero);
            TimeService.DeleteTask(_checkTaskID);
            _checkTaskID = 0;
        }

        private void OnSelectHero(EventSelectHero e)
        {
            if (e.RoomID != FSM.Room.RoomID) return;

            if (FSM.Room.HeroArr[e.Index].Comfirm) return;

            FSM.Room.HeroArr[e.Index].Comfirm = true;
            FSM.Room.HeroArr[e.Index].HeroID = e.HeroID;

            if (CheckComfirmDone())
            {
                TimeService.DeleteTask(_checkTaskID);
                Debug.ColorLog(LogColor.Green, $"所有玩家选择英雄完成,进入加载,RoomID: {FSM.Room.RoomID}");
                FSM.TransitionImmediately(EPvpState.Loading);
            }
        }

        private void ReachTimeLimit(int taskID)
        {
            if (CheckComfirmDone())
            {
                return;
            }
            else
            {
                for (int i = 0; i < FSM.Room.HeroArr.Length; i++)
                {
                    if (!FSM.Room.HeroArr[i].Comfirm)
                    {
                        FSM.Room.HeroArr[i].HeroID = 101;
                        FSM.Room.HeroArr[i].Comfirm = true;
                    }
                }
                FSM.TransitionImmediately(EPvpState.Loading);
            }
        }

        private void InitSelectData()
        {
            int len = FSM.Room.Sessions.Length;
            FSM.Room.HeroArr = new HeroSelectInfo[len];
            for (int i = 0; i < len; i++)
            {
                FSM.Room.HeroArr[i] = new HeroSelectInfo
                {
                    HeroID = 0,
                    Comfirm = false
                };
            }
        }

        private bool CheckComfirmDone()
        {
            for (int i = 0; i < FSM.Room.HeroArr.Length; i++)
            {
                if (!FSM.Room.HeroArr[i].Comfirm) return false;
            }

            return true;
        }
    }
}