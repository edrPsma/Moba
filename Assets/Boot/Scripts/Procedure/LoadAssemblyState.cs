using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using HybridCLR;
using UnityEngine;
using YooAsset;

public class LoadAssemblyState : IState
{
    public void Enter()
    {
#if !UNITY_EDITOR
        LoadMetadataForAOTAssemblies();
        LoadAssembly();
#else
        Boot.StateMachine.ChangeState(EBootState.EnterGame);
#endif
    }

    public void Update() { }

    public void Exit() { }

    void LoadAssembly()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);
        AssetHandle assetHandle = package.LoadAssetAsync<TextAsset>("Assets/GameAssets/Dll/HotUpdate.dll.bytes");
        assetHandle.Completed += handle =>
        {
            if (handle.Status == EOperationStatus.Succeed)
            {
                // 加载热更新程序集
                Assembly hotUpdateAss = Assembly.Load((handle.AssetObject as TextAsset).bytes);
            }
            else
            {
                Debug.LogError($"加载热更程序集失败: {handle.LastError}");
            }

            handle.Release();

            Boot.StateMachine.ChangeState(EBootState.EnterGame);
        };
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
