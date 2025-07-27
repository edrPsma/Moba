using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoginForm : UIForm
{
    public override UIGroup DefultGroup => UIGroup.Middle;
    public override string Location => "Assets/GameAssets/UIPrefab/LoginWnd.prefab";

    [Inject] ILoginController _loginController;

    protected override void OnStart()
    {
        this.Get<Button>("btnLogin").Subscribe(BtnLoginOnClick);
        this.Get<Toggle>("togSrv").Subscribe(OnServerTypeChange);
    }

    private void OnServerTypeChange(bool isOn)
    {

    }

    private void BtnLoginOnClick()
    {
        string acc = this.Get<InputField>("iptAcct").text;
        string pass = this.Get<InputField>("iptPass").text;
        _loginController.Login(acc, pass);
    }
}
