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
        TypeEventSource EventSource { get; }
    }

    [Reflection]
    public class MatchController : AbstractController, IMatchController
    {
        [Inject] public INetService NetService;
        Queue<ServerSession> _sessions;
        List<PvpFSM> _fsms;
        Dictionary<int, PvpRoom> _roomMap;
        int _roomID = 0;
        public TypeEventSource EventSource { get; private set; }

        protected override void OnInitialize()
        {
            base.OnInitialize();

            EventSource = new TypeEventSource();
            _roomMap = new Dictionary<int, PvpRoom>();
            _sessions = new Queue<ServerSession>();
            _fsms = new List<PvpFSM>();
            NetService.Register<U2GS_Match>(OnMatchReceive);
            NetService.Register<U2GS_Comfirm>(OnComfirm);
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

                PvpFSM pvpFSM = new PvpFSM(room);

                _fsms.Add(pvpFSM);
                pvpFSM.Initialize();
                pvpFSM.Enter();
                _roomMap.Add(room.RoomID, room);
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

        private void OnComfirm(ServerSession session, U2GS_Comfirm comfirm)
        {
            if (_roomMap.TryGetValue(comfirm.RoomID, out PvpRoom room))
            {
                room.Comfirm(session);
            }
            else
            {
                Debug.Warn($"该房间不存在,RoomID: {comfirm.RoomID}");
            }
        }
    }

    public class PvpRoom
    {
        public int RoomID;
        public ServerSession[] Sessions;
        public bool IsComplete;
        [Inject] public IMatchController MatchController;

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
            for (int i = 0; i < Sessions.Length; i++)
            {
                if (Sessions[i] == session)
                {
                    MatchController.EventSource.Trigger(new EventComfirm(RoomID, i));
                    break;
                }
            }
        }
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
}

