using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using YooAsset;

public interface IAssetSystem
{
    void Preload<T>(string location, Action callBack) where T : UnityEngine.Object;

    T Get<T>(string location) where T : UnityEngine.Object;
}

[Controller]
public class AssetSystem : AbstarctController, IAssetSystem
{
    Dictionary<string, UnityEngine.Object> _assetDic;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _assetDic = new Dictionary<string, UnityEngine.Object>();
    }

    public void Preload<T>(string location, Action callBack) where T : UnityEngine.Object
    {
        if (GameEntry.Resource.CheckLocationValid(location))
        {
            AssetHandle assetHandle = GameEntry.Resource.LoadAssetAsync<T>(location);
            assetHandle.Completed += handle =>
            {
                _assetDic.Add(location, handle.AssetObject);
                callBack?.Invoke();
            };
        }
        else
        {
            callBack?.Invoke();
        }
    }

    public T Get<T>(string location) where T : UnityEngine.Object
    {
        if (_assetDic.ContainsKey(location))
        {
            return _assetDic[location] as T;
        }
        else
        {
            return null;
        }
    }
}