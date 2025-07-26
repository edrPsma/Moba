using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class CreateDownloaderState : IState
{
    public void Enter()
    {
        CreateDownloader();
    }

    public void Update() { }

    public void Exit() { }

    void CreateDownloader()
    {
        int downloadingMaxNum = 10;
        int failedTryAgain = 3;
        var package = YooAssets.GetPackage(ConstantDefine.DefaultPackageName);
        var downloader = package.CreateResourceDownloader(downloadingMaxNum, failedTryAgain);

        //没有需要下载的资源
        if (downloader.TotalDownloadCount == 0)
        {
            Boot.StateMachine.ChangeState(EBootState.LoadAssembly);
            return;
        }
        else
        {
            float sizeMB = downloader.TotalDownloadBytes / 1048576f;
            sizeMB = Mathf.Clamp(sizeMB, 0.1f, float.MaxValue);
            string totalSizeMB = sizeMB.ToString("f1");
            EventShowTips eventShowTips = new EventShowTips
            {
                CallBack = () => Boot.StateMachine.ChangeState(EBootState.Download),
                Content = $"检测到更新, 文件总数量 {downloader.TotalDownloadCount} 文件总大小 {totalSizeMB}MB"
            };
            Boot.Event.TriggerEvent(eventShowTips);
        }
    }
}
