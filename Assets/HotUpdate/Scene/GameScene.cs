using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScene : BaseSceneState
{
    public override string DefultScenePath => "Assets/GameAssets/Scene/map_101.unity";
    public override LoadSceneMode SceneMode => LoadSceneMode.Additive;

    public Vector3 BlueSpawnPoint;
    public Vector3 RedSpawnPoint;
    public CameraFollow CameraFollow;

    protected override void OnSceneLoaded()
    {
        base.OnSceneLoaded();

        BlueSpawnPoint = FindRootObject("SpawnPoint").transform.Find("Blue").position;
        RedSpawnPoint = FindRootObject("SpawnPoint").transform.Find("Red").position;
        CameraFollow = FindRootObject("CameraRoot").GetComponent<CameraFollow>();
    }

    public FixIntVector3 GetSpawnPosition(EActorLayer layer)
    {
        if (layer == EActorLayer.Red)
        {
            return new FixIntVector3(RedSpawnPoint);
        }
        else if (layer == EActorLayer.Blue)
        {
            return new FixIntVector3(BlueSpawnPoint);
        }

        return FixIntVector3.zero;
    }
}
