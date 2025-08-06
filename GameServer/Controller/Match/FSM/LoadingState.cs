using System;
using GameServer.Common;
using GameServer.Service;
using Protocol;

namespace GameServer.Controller
{
    public class LoadingState : BasePvpState
    {
        [Inject] public ICacheService CacheService;
        LoadInfo[] _loadingInfos;

        public LoadingState(PvpFSM fsm, bool hasExitTime = true) : base(fsm, hasExitTime) { }

        protected override void OnEnter()
        {
            base.OnEnter();

            InitLoadingInfo();

            FSM.Room.OnRecover += OnRecover;

            GS2U_StartLoad msg = new GS2U_StartLoad();
            msg.RoomID = FSM.Room.RoomID;
            msg.LoadInfo.AddRange(_loadingInfos);
            FSM.Room.BroadcastMsg(msg);

            FSM.Room.EventSource.Register<EventLoading>(OnLoading);
        }

        protected override void OnExit()
        {
            base.OnExit();

            FSM.Room.OnRecover -= OnRecover;
            FSM.Room.EventSource.UnRegister<EventLoading>(OnLoading);
        }

        private void OnRecover(uint uid)
        {
            GS2U_StartLoad msg = new GS2U_StartLoad();
            msg.RoomID = FSM.Room.RoomID;
            int index = FSM.Room.GetIndex(uid);
            _loadingInfos[index].Progress = 0;
            msg.LoadInfo.AddRange(_loadingInfos);

            FSM.Room.Send(uid, msg);
        }

        private void OnLoading(EventLoading e)
        {
            if (e.RoomID != FSM.Room.RoomID) return;

            _loadingInfos[e.Index].Progress = e.Progress;

            SyncData();
            if (CheckLoadDone())
            {
                FSM.TransitionImmediately(EPvpState.Fight);
            }
        }

        void InitLoadingInfo()
        {
            int len = FSM.Room.Players.Length;
            _loadingInfos = new LoadInfo[len];
            for (int i = 0; i < len; i++)
            {
                _loadingInfos[i] = new LoadInfo
                {
                    UId = FSM.Room.Players[i],
                    Index = i,
                    Name = CacheService.GetPlayerName(FSM.Room.Players[i]),
                    Progress = 0,
                    HeroID = FSM.Room.HeroArr[i].HeroID,
                };
            }
        }

        bool CheckLoadDone()
        {
            for (int i = 0; i < _loadingInfos.Length; i++)
            {
                if (_loadingInfos[i].Progress < 100) return false;
            }

            return true;
        }

        void SyncData()
        {
            GS2U_Load msg = new GS2U_Load();
            msg.LoadInfo.AddRange(_loadingInfos);
            FSM.Room.BroadcastMsg(msg);
        }
    }
}