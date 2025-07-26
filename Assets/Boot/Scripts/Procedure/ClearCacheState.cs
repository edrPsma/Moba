using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class ClearCacheState : IState
{
    public void Enter()
    {
        ClearCache().Forget();
    }

    public void Update() { }

    public void Exit() { }

    async UniTaskVoid ClearCache()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);
        var operation = package.ClearCacheFilesAsync(EFileClearMode.ClearUnusedBundleFiles);
        await operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //清理成功

            Boot.StateMachine.ChangeState(EBootState.LoadAssembly);
        }
        else
        {
            //清理失败
            Debug.LogError(operation.Error);

            Boot.StateMachine.ChangeState(EBootState.LoadAssembly);
        }
    }
}
