using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class YooAssetInitializeState : IState
{
    public void Enter()
    {
        Boot.Event.TriggerEvent<EventInitialize>();

        // 初始化资源系统
        YooAssets.Initialize();

        // 创建默认的资源包
        var package = YooAssets.CreatePackage(ConstantDefine.DefaultPackageName);

        // 设置该资源包为默认的资源包，可以使用YooAssets相关加载接口加载该资源包内容。
        YooAssets.SetDefaultPackage(package);

        Boot.StateMachine.ChangeState(EBootState.YooAssetInitPackage);
    }

    public void Update() { }

    public void Exit() { }
}
