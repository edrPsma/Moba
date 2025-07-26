using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class CheckVersionState : IState
{
    public void Enter()
    {
        Boot.Event.TriggerEvent<EventCheckVersion>();
        CheckVersion().Forget();
    }

    public void Update() { }

    public void Exit() { }

    async UniTaskVoid CheckVersion()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);
        var operation = package.RequestPackageVersionAsync(false);
        await operation;

        if (operation.Status == EOperationStatus.Succeed)
        {
            //更新成功
            Boot.PackageVersion = operation.PackageVersion;
            Debug.Log($"Request package Version : {Boot.PackageVersion}");

            Boot.StateMachine.ChangeState(EBootState.UpdateManifest);
        }
        else
        {
            //更新失败
            Debug.LogError(operation.Error);

            EventShowTips eventShowTips = new EventShowTips
            {
                CallBack = () => Boot.StateMachine.ChangeState(EBootState.YooAssetInitPackage),
                Content = "检查更新失败"
            };
            Boot.Event.TriggerEvent(eventShowTips);
        }
    }
}
