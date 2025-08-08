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
        [Inject] public ICacheService CacheService;
        Queue<uint> _players;
        List<PvpFSM> _fsms;
        Dictionary<int, PvpRoom> _roomMap;
        int _roomID = 0;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _roomMap = new Dictionary<int, PvpRoom>();
            _players = new Queue<uint>();
            _fsms = new List<PvpFSM>();
            NetService.Register<U2GS_Match>(OnMatchReceive);
            NetService.Register<U2GS_Comfirm>(OnComfirm);
            NetService.Register<U2GS_SelectHero>(OnSelectHero);
            NetService.Register<U2GS_Load>(OnLoadChange);
            NetService.Register<U2GS_TryRecover>(OnTryRecover);
            NetService.Register<U2GS_Operate>(OnOperate);
            NetService.Register<U2GS_MissOperate>(OnMissOperate);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_players.Count >= 2)
            {
                uint[] sessions = new uint[2];
                sessions[0] = _players.Dequeue();
                sessions[1] = _players.Dequeue();

                PvpRoom room = Builder.NewAndInject<PvpRoom>();
                room.RoomID = GetRoomID();
                room.Players = sessions;
                room.EventSource = new TypeEventSource();
                room.Initialize();

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
            _players.Enqueue(session.UId);
        }

        private void OnComfirm(ServerSession session, U2GS_Comfirm msg)
        {
            if (_roomMap.TryGetValue(msg.RoomID, out PvpRoom room))
            {
                room.Comfirm(session.UId);
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
                room.SelectHero(session.UId, msg.HeroID);
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
                room.ChangeProgress(session.UId, msg.Progress);
            }
            else
            {
                Debug.Warn($"该房间不存在,RoomID: {msg.RoomID}");
            }
        }

        private void OnTryRecover(ServerSession session, U2GS_TryRecover msg)
        {
            int roomID = CacheService.GetRoom(session.UId);
            if (_roomMap.TryGetValue(roomID, out PvpRoom room))
            {
                room.Recover(session.UId);
            }
        }

        private void OnOperate(ServerSession session, U2GS_Operate operate)
        {
            int roomID = CacheService.GetRoom(session.UId);
            if (_roomMap.TryGetValue(roomID, out PvpRoom room))
            {
                room.AddOperate(session.UId, operate);
            }
            else
            {
                Debug.Warn($"该房间不存在,RoomID: {roomID}");
            }
        }

        private void OnMissOperate(ServerSession session, U2GS_MissOperate msg)
        {
            int roomID = CacheService.GetRoom(session.UId);
            if (_roomMap.TryGetValue(roomID, out PvpRoom room))
            {
                GS2U_MissOperate operate = new GS2U_MissOperate();
                int count = 0;
                for (int i = msg.StartID; i < msg.Count; i++)
                {
                    operate.Operates.Add(room.AllOperate[i]);
                    ++count;
                    if (count >= ServerConfig.ChaseFrameCount)
                    {
                        session.Send(operate);
                        count = 0;
                        operate.Operates.Clear();
                    }
                }
                if (operate.Operates.Count > 0)
                {
                    session.Send(operate);
                }
                Debug.ColorLog(LogColor.Green, $"向客户端发送丢失的帧,StartID: {msg.StartID} Count: {msg.Count} MaxCount:{room.AllOperate.Count}");
            }
            else
            {
                Debug.Warn($"该房间不存在,RoomID: {roomID}");
            }
        }

        private int GetRoomID()
        {
            _roomID++;
            if (_roomID == 0)
            {
                _roomID++;
            }

            return _roomID;
        }
    }

    public class PvpRoom
    {
        public int RoomID;
        public uint[] Players;
        public bool IsComplete;
        [Inject] public IMatchController MatchController;
        [Inject] public ICacheService CacheService;
        public HeroSelectInfo[] HeroArr;
        public TypeEventSource EventSource;
        public event Action<uint> OnRecover;
        public List<Operate> Operates = new List<Operate>();
        public List<GS2U_Operate> AllOperate = new List<GS2U_Operate>();
        HashSet<uint> _offlinePlayers = new HashSet<uint>();

        public void Initialize()
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (CacheService.TryGetSession(Players[i], out ServerSession session))
                {
                    session.OnDisConnectedEvent += uid =>
                    {
                        _offlinePlayers.Add(uid);
                    };
                }
            }
        }

        public void Recover(uint uid)
        {
            OnRecover?.Invoke(uid);
            if (CacheService.TryGetSession(uid, out ServerSession session))
            {
                session.OnDisConnectedEvent += _ =>
                {
                    _offlinePlayers.Add(uid);
                };
            }
            _offlinePlayers.Remove(uid);
        }

        public bool IsActive(uint uid)
        {
            return !_offlinePlayers.Contains(uid);
        }

        public void Send(uint uid, IMessage message)
        {
            if (CacheService.TryGetSession(uid, out ServerSession session))
            {
                session.Send(message);
            }
        }

        public void Send(uint uid, byte[] bytes)
        {
            if (CacheService.TryGetSession(uid, out ServerSession session))
            {
                session.Send(bytes);
            }
        }

        public void BroadcastMsg(IMessage message)
        {
            byte[] bytes = message.Serialize();

            for (int i = 0; i < Players.Length; i++)
            {
                Send(Players[i], bytes);
            }
        }

        public void Comfirm(uint uid)
        {
            int index = GetIndex(uid);
            if (index != -1)
            {
                EventSource.Trigger(new EventComfirm(RoomID, index));
            }
        }

        public void SelectHero(uint uid, int heroID)
        {
            int index = GetIndex(uid);
            if (index != -1)
            {
                EventSource.Trigger(new EventSelectHero(RoomID, index, heroID));
            }
        }

        public void ChangeProgress(uint uid, int progress)
        {
            int index = GetIndex(uid);
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

        public void AddOperate(uint uid, U2GS_Operate msg)
        {
            Operate operate = new Operate();
            operate.Type = msg.Type;
            operate.MoveOperate = msg.MoveOperate;
            operate.SkillOperate = msg.SkillOperate;
            operate.UId = uid;
            Operates.Add(operate);
        }

        public int GetIndex(uint uid)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                if (Players[i] == uid)
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

