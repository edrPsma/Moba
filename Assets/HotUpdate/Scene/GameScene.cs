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
    public DamageMark DamageMark;

    protected override void OnSceneLoaded()
    {
        base.OnSceneLoaded();

        BlueSpawnPoint = FindRootObject("SpawnPoint").transform.Find("Blue").position;
        RedSpawnPoint = FindRootObject("SpawnPoint").transform.Find("Red").position;
        CameraFollow = FindRootObject("CameraRoot").GetComponent<CameraFollow>();
        DamageMark = FindRootObject("Assets").GetComponentInChildren<DamageMark>(true);

        GameEntry.Scene.SetActiveScene<GameScene>();
    }

    public FixIntVector3 GetSpawnPosition(ECamp layer)
    {
        if (layer == ECamp.Red)
        {
            return new FixIntVector3(RedSpawnPoint);
        }
        else if (layer == ECamp.Blue)
        {
            return new FixIntVector3(BlueSpawnPoint);
        }

        return FixIntVector3.zero;
    }
}
