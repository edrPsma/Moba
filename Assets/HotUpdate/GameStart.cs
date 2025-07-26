using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Test;
using UI;
using UnityEngine;
using YooAsset;

public class GameStart : MonoBehaviour
{
    void Start()
    {
        GameEntry.Initialize(new GameFSM(), new UILoader());

        GameObject.Destroy(gameObject);
    }

    public class UILoader : IUILoader
    {
        ResourcePackage _package;

        public UILoader()
        {
            _package = YooAssets.GetPackage("DefaultPackage");
        }

        public void Load(string location, out object userData, Action<GameObject> loadOver)
        {
            AssetHandle handle = _package.LoadAssetSync<GameObject>(location);
            userData = handle;
            handle.Completed += handle =>
            {
                loadOver?.Invoke(GameObject.Instantiate(handle.AssetObject as GameObject));
            };
        }

        public void LoadAsync(string location, out object userData, Action<GameObject> loadOver)
        {
            AssetHandle handle = _package.LoadAssetSync<GameObject>(location);
            userData = handle;
            handle.Completed += handle =>
            {
                loadOver?.Invoke(GameObject.Instantiate(handle.AssetObject as GameObject));
            };
        }

        public void UnLoad(GameObject gameObject, object userData)
        {
            AssetHandle handle = userData as AssetHandle;

            GameObject.Destroy(gameObject);
            handle?.Release();
        }
    }
}
