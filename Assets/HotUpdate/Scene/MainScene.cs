using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : BaseSceneState
{
    public override string DefultScenePath => "Assets/GameAssets/Scene/main.unity";
    public override LoadSceneMode SceneMode => LoadSceneMode.Single;

    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Scene 进入主场景");

    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Scene 离开主场景");
    }
}
