using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class EnterGameState : IState
{
    public void Enter()
    {
        Debug.Log("进入游戏");

        LoadGameStartObject().Forget();
    }

    public void Update()
    {

    }

    public void Exit()
    {

    }

    async UniTaskVoid LoadGameStartObject()
    {
        AssetHandle handle = YooAssets.GetPackage(ConstantDefine.DefaultPackageName).LoadAssetAsync<GameObject>("Assets/GameAssets/GameStart.prefab");
        await handle.InstantiateAsync();

        Boot.Event.TriggerEvent<EventEnterGame>();
    }
}
