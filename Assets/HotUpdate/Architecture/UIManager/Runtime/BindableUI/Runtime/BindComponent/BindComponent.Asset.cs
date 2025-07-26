using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    public partial class BindComponent
    {
        Dictionary<string, int> _assetDic;
        public BindAssetData[] AssetDatas = new BindAssetData[0];

        void BuildAssetDic()
        {
            if (_assetDic != null) return;

            _assetDic = new Dictionary<string, int>();
            for (int i = 0; i < AssetDatas.Length; i++)
            {
                if (_assetDic.ContainsKey(AssetDatas[i].Name))
                {
                    Debug.LogError($"重复的资源名 Name: {AssetDatas[i].Name}", this);
                }
                else
                {
                    _assetDic.Add(AssetDatas[i].Name, i);
                }
            }
        }
    }

    [Serializable]
    public struct BindAssetData
    {
        public string Name;
        public UnityEngine.Object BindReference;
    }
}