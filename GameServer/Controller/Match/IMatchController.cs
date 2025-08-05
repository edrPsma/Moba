using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameServer.Common;
using GameServer.Service;
using Google.Protobuf;
using Observable;
using Protocol;

namespace GameServer.Controller
{
    public interface IMatchController
    {

    }

    [Reflection(typeof(IMatchController))]
    public class MatchController : AbstractController, IMatchController
    {
        [Inject] public INetService NetService;
        Queue<ServerSession> _sessions;
        List<PvpFSM> _fsms;
        Dictionary<int, PvpRoom> _roomMap;
        int _roomID = 0;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _roomMap = new Dictionary<int, PvpRoom>();
            _sessions = new Queue<ServerSession>();
            _fsms = new List<PvpFSM>();
            NetService.Register<U2GS_Match>(OnMatchReceive);
            NetService.Register<U2GS_Comfirm>(OnComfirm);
            NetService.Register<U2GS_SelectHero>(OnSelectHero);
            NetService.Register<U2GS_Load>(OnLoadChange);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_sessions.Count >= 2)
            {
                ServerSession[] sessions = new ServerSession[2];
                sessions[0] = _sessions.Dequeue();
                sessions[1] = _sessions.Dequeue();

                PvpRoom room = Builder.NewAndInject<PvpRoom>();
                room.RoomID = _roomID++;
                room.Sessions = sessions;
                room.EventSource = new TypeEventSource();

                PvpFSM pvpFSM = new PvpFSM(room);
                pvpFSM.Initialize();
                pvpFSM.Enter();

                _roomMap.Add(room.RoomID, room);
                _fsms.Add(pvpFSM);
            }

            for (int i = _fsms.Count - 1; i >= 0; i--)
            {
                if (_fsms[i].Room.IsComplete)
                {
                    PvpFSM pvpFSM = _fsms[i];
                    pvpFSM.Exit();
                    _fsms.Remove(pvpFSM);
                    _roomMap.Remove(pvpFSM.Room.RoomID);
                }
            }

            foreach (var item in _fsms)
            {
                item.Excute();
            }
        }

        private void OnMatchReceive(ServerSession session, U2GS_Match match)
        {
            _sessions.Enqueue(session);
        }

        private void OnComfirm(ServerSession session, U2GS_Comfirm msg)
        {
            if (_roomMap.TryGetValue(msg.RoomID, out PvpRoom room))
            {
                room.Comfirm(session);
            }
            else
            {
                Debug.Warn($"该房间不存在,RoomID: {msg.RoomID}");
            }
        }

        private void OnSelectHero(ServerSession session, U2GS_SelectHero msg)
        {
            if (_roomMap.TryGetValue(msg.RoomID, out PvpRoom room))
            {
                room.SelectHero(session, msg.HeroID);
            }
            else
            {
                Debug.Warn($"该房间不存在,RoomID: {msg.RoomID}");
            }
        }

        private void OnLoadChange(ServerSession session, U2GS_Load msg)
        {
            if (_roomMap.TryGetValue(msg.RoomID, out PvpRoom room))
            {
                room.ChangeProgress(session, msg.Progress);
            }
            else
            {
                Debug.Warn($"该房间不存在,RoomID: {msg.RoomID}");
            }
        }
    }

    public class PvpRoom
    {
        public int RoomID;
        public ServerSession[] Sessions;
        public bool IsComplete;
        [Inject] public IMatchController MatchController;
        public HeroSelectInfo[] HeroArr;
        public TypeEventSource EventSource;

        public void BroadcastMsg(IMessage message)
        {
            byte[] bytes = Sessions[0].Serialize(message);

            for (int i = 0; i < Sessions.Length; i++)
            {
                Sessions[i].Send(bytes);
            }
        }

        public void Comfirm(ServerSession session)
        {
            int index = GetIndex(session);
            if (index != -1)
            {
                EventSource.Trigger(new EventComfirm(RoomID, index));
            }
        }

        public void SelectHero(ServerSession session, int heroID)
        {
            int index = GetIndex(session);
            if (index != -1)
            {
                EventSource.Trigger(new EventSelectHero(RoomID, index, heroID));
            }
        }

        public void ChangeProgress(ServerSession session, int progress)
        {
            int index = GetIndex(session);
            if (index != -1)
            {
                EventSource.Trigger(new EventLoading(RoomID, index, progress));
            }
        }

        public void Fight()
        {
            GS2U_Battle msg = new GS2U_Battle();
            BroadcastMsg(msg);
        }

        private int GetIndex(ServerSession session)
        {
            for (int i = 0; i < Sessions.Length; i++)
            {
                if (Sessions[i] == session)
                {
                    return i;
                }
            }

            return -1;
        }
    }

    public class HeroSelectInfo
    {
        public int HeroID;
        public bool Comfirm;
    }

    public readonly struct EventComfirm
    {
        public int RoomID { get; }
        public int Index { get; }

        public EventComfirm(int roomID, int index)
        {
            RoomID = roomID;
            Index = index;
        }
    }

    public readonly struct EventSelectHero
    {
        public int RoomID { get; }
        public int Index { get; }
        public int HeroID { get; }

        public EventSelectHero(int roomID, int index, int heroID)
        {
            RoomID = roomID;
            Index = index;
            HeroID = heroID;
        }
    }

    public readonly struct EventLoading
    {
        public int RoomID { get; }
        public int Index { get; }
        public int Progress { get; }

        public EventLoading(int roomID, int index, int progress)
        {
            RoomID = roomID;
            Index = index;
            Progress = progress;
        }
    }
}

