using System.Collections;
using System.Collections.Generic;
using HFSM;
using Sirenix.OdinInspector;
using Test;
using UnityEngine;

public class LoginState : BaseState
{
    public LoginState(bool hasExitTime = false) : base(hasExitTime) { }

    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入登陆流程");

        GameEntry.Scene.LoadScene<MainScene>();
        GameEntry.UI.PushAsync<LoginForm>();
        GameEntry.Audio.PlayBGM("Assets/GameAssets/Audio/main.mp3");
    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出登陆流程");
    }

    [Button]
    void ShowA()
    {
        GameEntry.Audio.PlayEffect("Assets/GameAssets/Audio/victory.mp3");
    }

    [Button]
    void HideA1()
    {
        GameEntry.Audio.StopBGM();
    }

    [Button]
    void Relsease()
    {
        GameEntry.Resource.UnloadUnusedAssetsAsync();
    }
}
