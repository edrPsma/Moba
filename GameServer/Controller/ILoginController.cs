using System;
using System.Collections.Generic;
using GameServer.Common;
using GameServer.Service;
using Protocol;

namespace GameServer.Controller
{
    public interface ILoginController
    {

    }

    [Reflection(typeof(ILoginController))]
    public class LoginController : AbstractController, ILoginController
    {
        [Inject] public INetService NetService;
        [Inject] public ICacheService CacheService;

        protected override void OnInitialize()
        {
            NetService.Register<U2GS_Login>(OnLoginReceive);
        }

        private void OnLoginReceive(ServerSession session, U2GS_Login login)
        {
            if (CacheService.IsAcctOnline(login.Acct))
            {
                GS2U_Login msg = new GS2U_Login
                {
                    State = -1
                };
                session.Send(msg);
                Debug.Warn($"该玩家已登陆,Acct: {login.Acct}");
            }
            else
            {
                UserData userData = new UserData
                {
                    UId = session.SessionID,
                };
                userData.HeroList.AddRange(new List<int> { 1, 2 });
                CacheService.AcctOnline(login.Acct, session, userData);

                GS2U_Login msg = new GS2U_Login
                {
                    State = 0,
                    UserData = userData,
                };
                session.Send(msg);
            }
        }
    }
}