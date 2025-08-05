using System;
using GameServer.Common;
using GameServer.Service;
using Protocol;

namespace GameServer.Controller
{
    public class ConfirmState : BasePvpState
    {
        [Inject] public ITimeService TimeService;
        [Inject] public IMatchController MatchController;
        ComfirmData[] _comfirmDatas;
        int _checkTaskID;

        public ConfirmState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }

        protected override void OnEnter()
        {
            base.OnEnter();

            InitComfirmData();
            _checkTaskID = TimeService.AddTask(ServerConfig.ConfirmCountDown * 1000, ReachTimeLimit);
            MatchController.EventSource.Register<EventComfirm>(OnComfirm);
        }

        protected override void OnExit()
        {
            base.OnExit();
            MatchController.EventSource.UnRegister<EventComfirm>(OnComfirm);
            TimeService.DeleteTask(_checkTaskID);
            _checkTaskID = 0;
        }

        private void OnComfirm(EventComfirm comfirm)
        {
            if (comfirm.RoomID != FSM.Room.RoomID) return;

            if (_comfirmDatas[comfirm.Index].ComfirmDone) return;

            _comfirmDatas[comfirm.Index].ComfirmDone = true;

            if (CheckComfirmDone())
            {
                TimeService.DeleteTask(_checkTaskID);
                Debug.ColorLog(LogColor.Green, $"所有玩家确认完成,进入英雄选择,RoomID: {FSM.Room.RoomID}");
                FSM.TransitionImmediately(EPvpState.SelectHero);
            }
            SyncData(false);
        }

        private void ReachTimeLimit(int taskID)
        {
            if (CheckComfirmDone())
            {
                return;
            }
            else
            {
                Debug.ColorLog(LogColor.Green, $"超时 房间解散");
                SyncData(true);
                FSM.TransitionImmediately(EPvpState.End);
            }
        }

        private void InitComfirmData()
        {
            int len = FSM.Room.Sessions.Length;
            _comfirmDatas = new ComfirmData[len];
            for (int i = 0; i < len; i++)
            {
                _comfirmDatas[i] = new ComfirmData
                {
                    IconIndex = i,
                    ComfirmDone = false
                };
            }

            SyncData(false);
        }

        private void SyncData(bool dismiss)
        {
            GS2U_Comfirm msg = new GS2U_Comfirm
            {
                RoomID = FSM.Room.RoomID,
                Dismiss = dismiss,
            };
            msg.ComfirmArr.AddRange(_comfirmDatas);

            FSM.Room.BroadcastMsg(msg);
        }

        private bool CheckComfirmDone()
        {
            for (int i = 0; i < _comfirmDatas.Length; i++)
            {
                if (!_comfirmDatas[i].ComfirmDone) return false;
            }

            return true;
        }
    }
}