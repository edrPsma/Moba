using System;
using System.Collections.Generic;
using GameServer.Common;
using Protocol;

namespace GameServer.Service
{
    public interface ICacheService
    {
        bool IsAcctOnline(string acct);

        void AcctOnline(string acct, ServerSession session, UserData userData);
    }

    [Reflection(typeof(ICacheService))]
    public class CacheService : AbstractService, ICacheService
    {
        Dictionary<string, ServerSession> _acctDic;
        Dictionary<ServerSession, UserData> _dataDic;

        protected override void OnInitialize()
        {
            base.OnInitialize();

            _acctDic = new Dictionary<string, ServerSession>();
            _dataDic = new Dictionary<ServerSession, UserData>();
        }

        public bool IsAcctOnline(string acct)
        {
            return _acctDic.ContainsKey(acct);
        }

        public void AcctOnline(string acct, ServerSession session, UserData userData)
        {
            _acctDic.Add(acct, session);
            _dataDic.Add(session, userData);
        }
    }
}