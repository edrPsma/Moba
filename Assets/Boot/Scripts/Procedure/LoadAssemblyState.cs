using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using HybridCLR;
using UnityEngine;
using YooAsset;

public class LoadAssemblyState : IState
{
    public void Enter()
    {
#if !UNITY_EDITOR
        LoadMetadataForAOTAssemblies();
        LoadAssembly().Forget();
#else
        Boot.StateMachine.ChangeState(EBootState.EnterGame);
#endif
    }

    public void Update() { }

    public void Exit() { }

    async UniTaskVoid LoadAssembly()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);

        AssetHandle assetHandle = package.LoadAssetAsync<TextAsset>("Assets/GameAssets/Dll/Protocol.dll.bytes");
        AssetHandle assetHandle2 = package.LoadAssetAsync<TextAsset>("Assets/GameAssets/Dll/HotUpdate.dll.bytes");


        await UniTask.WhenAll(assetHandle2.ToUniTask(), assetHandle.ToUniTask());

        if (assetHandle.Status == EOperationStatus.Succeed)
        {
            // 加载热更新程序集
            Assembly.Load((assetHandle.AssetObject as TextAsset).bytes);
        }
        else
        {
            Debug.LogError($"加载热更程序集失败: {assetHandle.LastError}");
        }
        assetHandle.Release();


        if (assetHandle2.Status == EOperationStatus.Succeed)
        {
            // 加载热更新程序集
            Assembly.Load((assetHandle2.AssetObject as TextAsset).bytes);
        }
        else
        {
            Debug.LogError($"加载热更程序集失败: {assetHandle2.LastError}");
        }
        assetHandle2.Release();

        Boot.StateMachine.ChangeState(EBootState.EnterGame);


    }


    private static void LoadMetadataForAOTAssemblies()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);
        AllAssetsHandle handle = package.LoadAllAssetsAsync<TextAsset>("Assets/GameAssets/Dll/Aot/System.Core.dll.bytes");
        handle.Completed += allHandle =>
        {
            if (handle.Status == EOperationStatus.Succeed)
            {
                foreach (var item in handle.AllAssetObjects)
                {
                    byte[] dllBytes = (item as TextAsset).bytes;
                    LoadImageErrorCode err = RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
                    Debug.Log($"LoadMetadataForAOTAssembly:{item.name}. ret:{err}");
                }
            }
            else
            {
                Debug.LogError($"加载AOT程序集失败: {handle.LastError}");
            }

            allHandle.Release();
        };
    }
}
