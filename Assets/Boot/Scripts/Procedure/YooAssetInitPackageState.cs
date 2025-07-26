using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class YooAssetInitPackageState : IState
{
    public void Enter()
    {
        Boot.Event.TriggerEvent<EventInitPackage>();

        if (Boot.PlayMode == EPlayMode.EditorSimulateMode)
        {
            EditorMode().Forget();
        }
        else if (Boot.PlayMode == EPlayMode.WebPlayMode)
        {
            WebGLMode().Forget();
        }
        else if (Boot.PlayMode == EPlayMode.HostPlayMode)
        {
            HostPlayMode().Forget();
        }
        else
        {
            Debug.LogError($"不支持的运行模式：{Boot.PlayMode}");
        }
    }

    public void Update() { }

    public void Exit() { }

    async UniTaskVoid EditorMode()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);
        var buildResult = EditorSimulateModeHelper.SimulateBuild(ConstantDefine.DefaultPackageName);
        var packageRoot = buildResult.PackageRootDirectory;
        Debug.Log("资源包根目录：" + packageRoot);
        var editorFileSystemParams = FileSystemParameters.CreateDefaultEditorFileSystemParameters(packageRoot);
        var initParameters = new EditorSimulateModeParameters();
        initParameters.EditorFileSystemParameters = editorFileSystemParams;
        var initOperation = package.InitializeAsync(initParameters);

        await initOperation;

        if (initOperation.Status == EOperationStatus.Succeed)
        {
            Debug.Log("资源包初始化成功！");

            Boot.StateMachine.ChangeState(EBootState.CheckVersion);
        }
        else
        {
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");

            ShowFailTips();
        }
    }

    async UniTaskVoid WebGLMode()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);

        //说明：RemoteServices类定义请参考联机运行模式！
        IRemoteServices remoteServices = new RemoteServices(Boot.ResServer, Boot.ResServer);
        var webServerFileSystemParams = FileSystemParameters.CreateDefaultWebServerFileSystemParameters();
        var webRemoteFileSystemParams = FileSystemParameters.CreateDefaultWebRemoteFileSystemParameters(remoteServices); //支持跨域下载

        var initParameters = new WebPlayModeParameters();
        initParameters.WebServerFileSystemParameters = null;
        initParameters.WebRemoteFileSystemParameters = webRemoteFileSystemParams;

        var initOperation = package.InitializeAsync(initParameters);
        await initOperation;

        if (initOperation.Status == EOperationStatus.Succeed)
        {
            Debug.Log("资源包初始化成功！");

            Boot.StateMachine.ChangeState(EBootState.CheckVersion);
        }
        else
        {
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");

            ShowFailTips();
        }
    }

    async UniTaskVoid HostPlayMode()
    {
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);

        IRemoteServices remoteServices = new RemoteServices(Boot.ResServer, Boot.ResServer);
        var cacheFileSystemParams = FileSystemParameters.CreateDefaultCacheFileSystemParameters(remoteServices);
        var buildinFileSystemParams = FileSystemParameters.CreateDefaultBuildinFileSystemParameters();

        var initParameters = new HostPlayModeParameters();
        initParameters.BuildinFileSystemParameters = null;
        initParameters.CacheFileSystemParameters = cacheFileSystemParams;
        var initOperation = package.InitializeAsync(initParameters);
        await initOperation;

        if (initOperation.Status == EOperationStatus.Succeed)
        {
            Debug.Log("资源包初始化成功！");

            Boot.StateMachine.ChangeState(EBootState.CheckVersion);
        }
        else
        {
            Debug.LogError($"资源包初始化失败：{initOperation.Error}");

            ShowFailTips();
        }
    }

    void ShowFailTips()
    {
        EventShowTips eventShowTips = new EventShowTips
        {
            CallBack = () => Boot.StateMachine.ChangeState(EBootState.YooAssetInitPackage),
            Content = "资源包初始化失败"
        };
        Boot.Event.TriggerEvent(eventShowTips);
    }
}
