using System.Collections;
using System.Collections.Generic;
using HFSM;
using Sirenix.OdinInspector;
using UnityEngine;
using YooAsset;
using Zenject;

public class LoginState : BaseState
{
    [Inject] public IPlayerModel PlayerModel;

    public LoginState(bool hasExitTime = false) : base(hasExitTime) { }

    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入登陆流程");

        AssetHandle assetHandle = GameEntry.Resource.LoadAssetAsync<GameConfig>("Assets/GameAssets/So/GameConfig.asset");
        assetHandle.Completed += handle =>
        {
            GameConfig gameConfig = handle.AssetObject as GameConfig;
            PlayerModel.GameConfig = gameConfig;
            GameEntry.Net.InitNet(gameConfig.RemoteIP, gameConfig.Port);
        };
        GameEntry.Scene.LoadScene<MainScene>();
        GameEntry.UI.PushAsync<LoginForm>();
        GameEntry.Audio.PlayBGM("Assets/GameAssets/Audio/main.mp3");
    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出登陆流程");

        GameEntry.UI.Pop<LoginForm>();
    }
}
