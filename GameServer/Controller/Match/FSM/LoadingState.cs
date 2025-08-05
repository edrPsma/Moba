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
            GS2U_StartLoad msg = new GS2U_StartLoad();
            msg.LoadInfo.AddRange(_loadingInfos);
            FSM.Room.BroadcastMsg(msg);

            FSM.Room.EventSource.Register<EventLoading>(OnLoading);
        }

        protected override void OnExit()
        {
            base.OnExit();

            FSM.Room.EventSource.UnRegister<EventLoading>(OnLoading);
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
            int len = FSM.Room.Sessions.Length;
            _loadingInfos = new LoadInfo[len];
            for (int i = 0; i < len; i++)
            {
                _loadingInfos[i] = new LoadInfo
                {
                    UId = FSM.Room.Sessions[i].SessionID,
                    Index = i,
                    Name = CacheService.GetPlayerName(FSM.Room.Sessions[i]),
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