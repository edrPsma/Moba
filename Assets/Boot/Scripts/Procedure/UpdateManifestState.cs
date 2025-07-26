using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class UpdateManifestState : IState
{
    public void Enter()
    {
        Boot.Event.TriggerEvent<EventUpdateManifest>();
        UpdateManifest().Forget();
    }

    public void Update() { }

    public void Exit() { }

    async UniTaskVoid UpdateManifest()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);
        var operation = package.UpdatePackageManifestAsync(Boot.PackageVersion);
        await operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //更新成功

            Boot.StateMachine.ChangeState(EBootState.CreateDownloader);

            Debug.Log($"Update package manifest succeed, new version: {Boot.PackageVersion}");
        }
        else
        {
            //更新失败
            Debug.LogError(operation.Error);

            EventShowTips eventShowTips = new EventShowTips
            {
                CallBack = () => Boot.StateMachine.ChangeState(EBootState.YooAssetInitPackage),
                Content = "更新资源清单失败"
            };
            Boot.Event.TriggerEvent(eventShowTips);
        }
    }
}
