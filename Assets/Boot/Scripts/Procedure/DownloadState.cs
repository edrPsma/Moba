using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using YooAsset;

public class DownloadState : IState
{
    public void Enter()
    {
        Boot.Event.TriggerEvent<EventDownload>();
        if (Boot.PlayMode == EPlayMode.EditorSimulateMode)
        {
            Boot.StateMachine.ChangeState(EBootState.ClearCache);
        }
        else if (Boot.PlayMode == EPlayMode.WebPlayMode)
        {
            Download().Forget();
        }
        else if (Boot.PlayMode == EPlayMode.HostPlayMode)
        {
            Download().Forget();
        }
        else
        {
            Debug.LogError($"不支持的运行模式：{Boot.PlayMode}");
        }
    }

    public void Update() { }

    public void Exit() { }

    async UniTaskVoid Download()
    {
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);
        var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        //需要下载的文件总数和总大小
        int totalDownloadCount = downloader.TotalDownloadCount;
        long totalDownloadBytes = downloader.TotalDownloadBytes;

        //注册回调方法
        downloader.DownloadUpdateCallback = OnDownloadUpdateFunction; //当下载进度发生变化

        Debug.Log("开始下载资源...");

        //开启下载
        downloader.BeginDownload();
        await downloader;

        //检测下载结果
        if (downloader.Status == EOperationStatus.Succeed)
        {
            //下载成功
            Boot.StateMachine.ChangeState(EBootState.ClearCache);

            Debug.Log($"Download succeed, total count: {totalDownloadCount}, total size: {totalDownloadBytes} bytes");
        }
        else
        {
            //下载失败

            EventShowTips eventShowTips = new EventShowTips
            {
                CallBack = () => Boot.StateMachine.ChangeState(EBootState.Download),
                Content = "资源下载失败"
            };
            Boot.Event.TriggerEvent(eventShowTips);
        }
    }

    private void OnDownloadUpdateFunction(DownloadUpdateData data)
    {
        //下载进度发生变化
        Debug.Log($"Downloading... current count: {data.CurrentDownloadCount}, current size: {data.CurrentDownloadBytes} bytes");

        EventDownloadProgress eventDownloadProgress = new EventDownloadProgress
        {
            CurrentDownloadCount = data.CurrentDownloadCount,
            TotalDownloadCount = data.TotalDownloadCount,
            CurrentDownloadSizeBytes = data.CurrentDownloadBytes,
            TotalDownloadSizeBytes = data.TotalDownloadBytes
        };
        Boot.Event.TriggerEvent(eventDownloadProgress);
    }
}
