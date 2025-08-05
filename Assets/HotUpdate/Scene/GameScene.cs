using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : BaseSceneState
{
    public override string DefultScenePath => "Assets/GameAssets/Scene/map_101.unity";
    public override LoadSceneMode SceneMode => LoadSceneMode.Additive;

    protected override void OnEnter()
    {
        base.OnEnter();
    }
}
