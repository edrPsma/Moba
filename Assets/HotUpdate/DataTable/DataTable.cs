using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using Observable;
using UnityEngine;
using YooAsset;

public static partial class DataTable
{
    static Dictionary<Type, object> _configDic;
    public static FloatVariable Progress { get; private set; }
    static bool _initDone = false;
    static int _count;
    static int _totalCount;

    public static void Initialize()
    {
        if (_initDone) return;

        _initDone = true;
        _configDic = new Dictionary<Type, object>();
        Progress = new FloatVariable();
        Register();
    }

    public static T GetItem<T>(string key) where T : class, IDataTable, new()
    {
        Type type = typeof(T);
        if (!_configDic.ContainsKey(type))
        {
            Debug.LogError($"DataTable 没有该配置表,Type:{type}");
            return null;
        }
        else
        {
            var dic = _configDic[type] as Dictionary<string, T>;
            return dic.GetValue(key.ToString());
        }
    }

    public static T GetItem<T>(int key) where T : class, IDataTable, new()
    {
        return GetItem<T>(key.ToString());
    }

    public static T GetItem<T>(long key) where T : class, IDataTable, new()
    {
        return GetItem<T>(key.ToString());
    }

    public static T[] GetArray<T>() where T : class, IDataTable, new()
    {
        Type type = typeof(T);
        if (!_configDic.ContainsKey(type))
        {
            Debug.LogError($"DataTable 没有该配置表,Type:{type}");
            return null;
        }
        else
        {
            var dic = _configDic[type] as Dictionary<string, T>;
            return dic.Values.OfType<T>().ToArray();
        }
    }

    public static IEnumerable<T> GetEnumerator<T>() where T : class, IDataTable, new()
    {
        Type type = typeof(T);
        if (!_configDic.ContainsKey(type))
        {
            Debug.LogError($"DataTable 没有该配置表,Type:{type}");
            return null;
        }
        else
        {
            var dic = _configDic[type] as Dictionary<string, T>;
            return dic.Values.OfType<T>();
        }
    }

    public static Dictionary<string, T> GetDic<T>() where T : class, IDataTable, new()
    {
        Type type = typeof(T);
        if (!_configDic.ContainsKey(type))
        {
            Debug.LogError($"DataTable 没有该配置表,Type:{type}");
            return null;
        }
        else
        {
            var dic = _configDic[type] as Dictionary<string, T>;
            return dic;
        }
    }

    public static bool ContainsKey<T>(string key) where T : class, IDataTable, new()
    {
        Type type = typeof(T);
        if (!_configDic.ContainsKey(type))
        {
            Debug.LogError($"DataTable 没有该配置表,Type:{type}");
            return false;
        }
        else
        {
            var dic = _configDic[type] as Dictionary<string, T>;
            return dic.ContainsKey(key.ToString());
        }
    }

    public static bool ContainsKey<T>(int key) where T : class, IDataTable, new()
    {
        return ContainsKey<T>(key.ToString());
    }

    static partial void Register();

    static void Load<T>() where T : class, IDataTable, new()
    {
        Type type = typeof(T);
        string location = $"Assets/GameAssets/Config/{type.Name}.json";

        if (GameEntry.Resource.CheckLocationValid(location))
        {
            AssetHandle assetHandle = GameEntry.Resource.LoadAssetAsync<TextAsset>($"Assets/GameAssets/Config/{type.Name}.json");
            assetHandle.Completed += handler =>
            {
                TextAsset textAsset = handler.AssetObject as TextAsset;
                var obj = JsonMapper.ToObject<Dictionary<string, T>>(textAsset.text);

                _configDic.Add(type, obj);
                handler.Release();
                _count++;
                Progress.Value = _count / _totalCount;
            };
        }
        else
        {
            _count++;
            Progress.Value = _count / _totalCount;
            Debug.LogError($"DataTable 加载失败,未找到配置表: {type.Name}");
        }
    }
}
