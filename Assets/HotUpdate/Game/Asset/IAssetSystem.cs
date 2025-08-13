using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using YooAsset;

public interface IAssetSystem
{
    void Preload<T>(string location, Action callBack) where T : UnityEngine.Object;

    void PreloadHeroAsset(string location, Action callBack);

    T Get<T>(string location) where T : UnityEngine.Object;

    T GetHeroAsset<T>(string location) where T : UnityEngine.Object;

    SkillConfig GetSkillConfig(int skillID);

    void Dispose();
}

[Controller]
public class AssetSystem : AbstarctController, IAssetSystem
{
    Dictionary<string, UnityEngine.Object> _assetDic;
    Dictionary<string, AssetHandle> _handleDic;
    Dictionary<string, UnityEngine.Object> _heroAssetDic;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        _assetDic = new Dictionary<string, UnityEngine.Object>();
        _handleDic = new Dictionary<string, AssetHandle>();
        _heroAssetDic = new Dictionary<string, UnityEngine.Object>();
    }

    public void Preload<T>(string location, Action callBack) where T : UnityEngine.Object
    {
        if (GameEntry.Resource.CheckLocationValid(location) && !_handleDic.ContainsKey(location))
        {
            AssetHandle assetHandle = GameEntry.Resource.LoadAssetAsync<T>(location);
            _handleDic.Add(location, assetHandle);
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

    public void PreloadHeroAsset(string location, Action callBack)
    {
        if (GameEntry.Resource.CheckLocationValid(location) && !_handleDic.ContainsKey(location))
        {
            AssetHandle assetHandle = GameEntry.Resource.LoadAssetAsync<HeroAsset>(location);
            _handleDic.Add(location, assetHandle);
            assetHandle.Completed += handle =>
            {
                HeroAsset heroAsset = handle.AssetObject as HeroAsset;
                foreach (var item in heroAsset.Assets)
                {
                    _heroAssetDic.SetValue(item.Path, item.Asset);
                }

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

    public T GetHeroAsset<T>(string location) where T : UnityEngine.Object
    {
        if (_heroAssetDic.ContainsKey(location))
        {
            return _heroAssetDic[location] as T;
        }
        else
        {
            return null;
        }
    }

    public SkillConfig GetSkillConfig(int skillID)
    {
        DTSkill table = DataTable.GetItem<DTSkill>(skillID);
        return Get<SkillConfig>($"Assets/GameAssets/So/Skill/{table.Config}.asset");
    }

    public void Dispose()
    {
        foreach (var item in _assetDic)
        {
            GameObject.Destroy(item.Value);
        }

        foreach (var item in _handleDic)
        {
            item.Value.Release();
        }

        _assetDic.Clear();
        _handleDic.Clear();
    }
}