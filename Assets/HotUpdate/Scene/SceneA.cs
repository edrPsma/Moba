using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneA : BaseSceneState
{
    public override string DefultScenePath => "Assets/GameAssets/Scene/SceneA.unity";
    public override LoadSceneMode SceneMode => LoadSceneMode.Single;

    protected override void OnEnter()
    {
        Debug.Log("SceneA Enter");
    }

    protected override void OnExit()
    {
        Debug.Log("SceneA Exit");
    }
}

public class SceneB : BaseSceneState
{
    public override string DefultScenePath => "Assets/GameAssets/Scene/SceneB.unity";
    public override LoadSceneMode SceneMode => LoadSceneMode.Single;

    protected override void OnEnter()
    {
        Debug.Log("SceneB Enter");
    }

    protected override void OnExit()
    {
        Debug.Log("SceneB Exit");
    }
}

public class SceneA1 : BaseSceneState
{
    public override string DefultScenePath => "Assets/GameAssets/Scene/SceneA1.unity";
    public override LoadSceneMode SceneMode => LoadSceneMode.Additive;

    protected override void OnEnter()
    {
        Debug.Log("SceneA1 Enter");
    }

    protected override void OnExit()
    {
        Debug.Log("SceneA1 Exit");
    }
}

public class SceneA2 : BaseSceneState
{
    public override string DefultScenePath => "Assets/GameAssets/Scene/SceneA2.unity";
    public override LoadSceneMode SceneMode => LoadSceneMode.Additive;

    protected override void OnEnter()
    {
        Debug.Log("SceneA2 Enter");
    }

    protected override void OnExit()
    {
        Debug.Log("SceneA2 Exit");
    }
}