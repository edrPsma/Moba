using System;
using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;
using Zenject;

public interface ILoginController
{
    void Login(string acc, string pass);
}

[Controller]
public class LoginController : AbstarctController, ILoginController
{
    [Inject] public IPlayerModel PlayerModel;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        GameEntry.Net.Register<GS2U_Login>(OnLoginReceive);
    }

    public void Login(string acc, string pass)
    {
        if (acc.Length < 3 || pass.Length < 3)
        {
            TipsForm.ShowTips("账号或密码错误");
        }
        else
        {
            GameEntry.Net.Connect(result => OnConnectComplete(result, acc, pass));
        }

    }

    private void OnConnectComplete(bool result, string acct, string pass)
    {
        if (result)
        {
            Debug.Log("连接服务器成功!");
            U2GS_Login msg = new U2GS_Login
            {
                Acct = acct,
                Pass = pass,
            };

            GameEntry.Net.SendMsg(msg);
        }
        else
        {
            TipsForm.ShowTips("无法连接服务器,请检查你的网络");
        }
    }

    private void OnLoginReceive(GS2U_Login login)
    {
        if (login.State == -1)
        {
            TipsForm.ShowTips("账号已登陆");
        }
        else
        {
            PlayerModel.UID = login.UserData.UId;
            PlayerModel.HeroList.AddRange(login.UserData.HeroList);
            TipsForm.ShowTips("登陆成功");
        }
    }
}