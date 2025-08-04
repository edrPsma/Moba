using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GameServer.Common;
using GameServer.Service;
using Protocol;

namespace GameServer.Controller
{
    public interface IMatchController
    {

    }

    public class MatchController : AbstractController, IMatchController
    {
        [Inject] public INetService NetService;
        Queue<ServerSession> _sessions = new Queue<ServerSession>();
        List<PvpRoom> _rooms = new List<PvpRoom>();
        Dictionary<int, PvpRoom> _roomMap = new Dictionary<int, PvpRoom>();
        int _roomID = 0;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _sessions = new Queue<ServerSession>();
            NetService.Register<U2GS_Match>(OnMatchReceive);
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            if (_sessions.Count >= 2)
            {
                ServerSession[] sessions = new ServerSession[2];
                sessions[0] = _sessions.Dequeue();
                sessions[1] = _sessions.Dequeue();

                PvpRoom room = new PvpRoom(_roomID++, sessions);
                _rooms.Add(room);
                _roomMap.Add(room.RoomID, room);
            }
        }

        private void OnMatchReceive(ServerSession session, U2GS_Match match)
        {
            _sessions.Enqueue(session);
        }
    }

    public class PvpRoom
    {
        public int RoomID;
        public ServerSession[] Sessions;

        public PvpRoom(int roomID, ServerSession[] sessions)
        {
            RoomID = roomID;
            Sessions = sessions;
        }
    }
}

