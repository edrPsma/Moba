using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;
using Zenject;

public class LoadingGameState : BaseState
{
    public LoadingGameState(bool hasExitTime = false) : base(hasExitTime) { }

    [Inject] public IMatchController MatchController;
    int _progress;
    GameScene _gameScene;
    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入加载游戏流程");

        GameEntry.UI.PushAsync<LoadingGameForm>();
        GameEntry.UI.PushAsync<PlayForm>();

        // 加载游戏资源
        GameEntry.Scene.LoadScene<GameScene>();
        _gameScene = GameEntry.Scene.GetSceneState<GameScene>();
    }

    protected override void OnExcute()
    {
        base.OnExcute();
        if (_progress != _gameScene.Progress)
        {
            _progress = _gameScene.Progress;
            Debug.Log(_progress);
            MatchController.LoadChange(_progress);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出加载游戏流程");
    }
}
