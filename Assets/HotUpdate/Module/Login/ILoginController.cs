using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface ILoginController : IController
{
    void Login(string acc, string pass);
}

[Controller(typeof(ILoginController))]
public class LoginController : ILoginController
{
    public void Login(string acc, string pass)
    {
        if (acc.Length < 3 || pass.Length < 3)
        {
            TipsForm.ShowTips("账号或密码错误");
        }
        else
        {
            // TODO 业务处理
            TipsForm.ShowTips("登陆成功");
        }

    }
}