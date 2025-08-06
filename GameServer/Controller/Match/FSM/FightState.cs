using System;
using GameServer.Common;
using GameServer.Service;
using Protocol;

namespace GameServer.Controller
{
    public class FightState : BasePvpState
    {
        [Inject] public ITimeService TimeService;
        [Inject] public ICacheService CacheService;
        int _checkTaskID;

        public FightState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }

        protected override void OnEnter()
        {
            base.OnEnter();

            FSM.Room.OnRecover += OnRecover;
            _checkTaskID = TimeService.AddTask(ServerConfig.FightCountDown * 1000, ReachTimeLimit);
            FSM.Room.Fight();

            FSM.Room.EventSource.Register<EventLoading>(OnLoading);
        }

        protected override void OnExit()
        {
            base.OnExit();
            FSM.Room.OnRecover -= OnRecover;
            FSM.Room.EventSource.UnRegister<EventLoading>(OnLoading);
            TimeService.DeleteTask(_checkTaskID);
            _checkTaskID = 0;
        }

        private void OnRecover(uint uid)
        {
            GS2U_StartLoad msg = new GS2U_StartLoad();
            msg.RoomID = FSM.Room.RoomID;
            int index = FSM.Room.GetIndex(uid);

            int len = FSM.Room.Players.Length;
            var loadingInfos = new LoadInfo[len];
            for (int i = 0; i < len; i++)
            {
                loadingInfos[i] = new LoadInfo
                {
                    UId = FSM.Room.Players[i],
                    Index = i,
                    Name = CacheService.GetPlayerName(FSM.Room.Players[i]),
                    Progress = i == index ? 0 : 100,
                    HeroID = FSM.Room.HeroArr[i].HeroID,
                };
            }

            msg.LoadInfo.AddRange(loadingInfos);

            FSM.Room.Send(uid, msg);
        }

        private void OnLoading(EventLoading e)
        {
            if (e.RoomID != FSM.Room.RoomID) return;

            GS2U_Battle msg = new GS2U_Battle();

            if (e.Progress >= 100)
            {
                FSM.Room.Send(FSM.Room.Players[e.Index], msg);
            }
        }

        private void ReachTimeLimit(int taskID)
        {
            Debug.ColorLog(LogColor.Blue, $"达到最大战斗时常 自动结束战斗,Room: {FSM.Room.RoomID}");
            FSM.TransitionImmediately(EPvpState.End);
        }
    }
}