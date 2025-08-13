using System;
using System.Collections.Generic;
using GameServer.Common;
using Protocol;

namespace GameServer.Service
{
    public interface ICacheService
    {
        bool IsAcctOnline(string acct);

        bool IsOnline(uint uid);

        void AcctOnline(string acct, ServerSession session, UserData userData);

        string GetPlayerName(uint uid);

        void Offline(uint uid);

        bool TryGetSession(uint uid, out ServerSession session);

        bool IsInGame(uint uid);

        void SetRoom(uint uid, int roomID);

        int GetRoom(uint uid);
    }

    [Reflection(typeof(ICacheService))]
    public class CacheService : AbstractService, ICacheService
    {
        Dictionary<string, uint> _acctDic;
        Dictionary<uint, ServerSession> _sessionDic;
        Dictionary<uint, UserData> _dataDic;
        Dictionary<uint, int> _gameDic;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _acctDic = new Dictionary<string, uint>();
            _dataDic = new Dictionary<uint, UserData>();
            _sessionDic = new Dictionary<uint, ServerSession>();
            _gameDic = new Dictionary<uint, int>();
        }

        public bool IsAcctOnline(string acct)
        {
            if (_acctDic.TryGetValue(acct, out uint uid))
            {
                return _sessionDic.ContainsKey(uid);
            }

            return false;
        }

        public bool IsOnline(uint uid)
        {
            return _sessionDic.ContainsKey(uid);
        }

        public void AcctOnline(string acct, ServerSession session, UserData userData)
        {
            if (!_acctDic.ContainsKey(acct))
            {
                _acctDic.Add(acct, userData.UId);
            }
            if (!_dataDic.ContainsKey(userData.UId))
            {
                _dataDic.Add(userData.UId, userData);
            }
            if (!_gameDic.ContainsKey(userData.UId))
            {
                _gameDic.Add(userData.UId, 0);
            }
            _sessionDic.Add(userData.UId, session);
            session.UId = userData.UId;
        }

        public string GetPlayerName(uint uid)
        {
            if (_dataDic.ContainsKey(uid))
            {
                return _dataDic[uid].Name;
            }
            else
            {
                return string.Empty;
            }
        }

        public void Offline(uint uid)
        {
            _sessionDic.Remove(uid);
        }

        public bool TryGetSession(uint uid, out ServerSession session)
        {
            return _sessionDic.TryGetValue(uid, out session);
        }

        public bool IsInGame(uint uid)
        {
            if (_gameDic.ContainsKey(uid))
            {
                return _gameDic[uid] != 0;
            }
            else
            {
                return false;
            }
        }

        public void SetRoom(uint uid, int roomID)
        {
            if (_gameDic.ContainsKey(uid))
            {
                _gameDic[uid] = roomID;
            }
        }

        public int GetRoom(uint uid)
        {
            if (_gameDic.ContainsKey(uid))
            {
                return _gameDic[uid];
            }
            else
            {
                return 0;
            }
        }
    }
}