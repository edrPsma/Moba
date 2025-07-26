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
        Debug.Log("进入登陆流程");

        UserInfo testInfo = new UserInfo();
        Debug.Log("测试协议是否正确加载: " + testInfo?.GetType());
        GameEntry.UI.PushAsync<LoginForm>();
    }

    [Button]
    void ShowA()
    {
        GameEntry.Scene.LoadScene<SceneA>();
    }

    [Button]
    void ShowB()
    {
        GameEntry.Scene.LoadScene<SceneB>();
    }

    [Button]
    void ShowA1()
    {
        GameEntry.Scene.LoadScene<SceneA1>();
    }

    [Button]
    void ShowA2()
    {
        GameEntry.Scene.LoadScene<SceneA2>();
    }

    [Button]
    void HideA1()
    {
        GameEntry.Scene.UnloadChildScene<SceneA1>();
    }

    [Button]
    void HideA2()
    {
        GameEntry.Scene.UnloadChildScene<SceneA2>();
    }

    [Button]
    void Relsease()
    {
        GameEntry.Resource.UnloadUnusedAssetsAsync();
    }
}
