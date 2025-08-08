using System;
using System.Collections.Generic;
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
        int _gameTaskID;
        int _frameID = 0;

        public FightState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }

        protected override void OnEnter()
        {
            base.OnEnter();

            FSM.Room.OnRecover += OnRecover;
            _checkTaskID = TimeService.AddTask(ServerConfig.FightCountDown * 1000, ReachTimeLimit);
            FSM.Room.Fight();
            _frameID = 0;
            _gameTaskID = TimeService.AddTask(ServerConfig.LogicFrameInterval, LogicUpdate, null, 0);
            FSM.Room.EventSource.Register<EventLoading>(OnLoading);
        }

        private void LogicUpdate(int obj)
        {
            GS2U_Operate msg = new GS2U_Operate();

            msg.FrameID = _frameID;
            msg.Operates.AddRange(FSM.Room.Operates);
            FSM.Room.AllOperate.Add(msg);

            byte[] bytes = msg.Serialize();
            for (int i = 0; i < FSM.Room.Players.Length; i++)
            {
                uint uid = FSM.Room.Players[i];
                if (FSM.Room.IsActive(uid))
                {
                    FSM.Room.Send(uid, bytes);
                }
            }
            FSM.Room.Operates.Clear();
            _frameID++;
        }

        protected override void OnExit()
        {
            base.OnExit();
            FSM.Room.OnRecover -= OnRecover;
            FSM.Room.EventSource.UnRegister<EventLoading>(OnLoading);
            TimeService.DeleteTask(_checkTaskID);
            TimeService.DeleteTask(_gameTaskID);
            _gameTaskID = 0;
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