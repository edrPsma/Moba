using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

public class ResourceUnloaderTrigger : MonoBehaviour
{
    HashSet<HandleBase> _assetHandles = new HashSet<HandleBase>();

    public void AddAssetHandle(HandleBase handle)
    {
        if (handle.IsValid)
        {
            _assetHandles.Add(handle);
        }
    }

    void OnDestroy()
    {
        foreach (var item in _assetHandles)
        {
            if (item.IsValid)
            {
                item.Release();
            }
        }
        _assetHandles = null;
    }
}

public static class ResourceUnloaderTriggerExtension
{
    public static T Bind<T>(this T handle, GameObject gameObject) where T : HandleBase
    {
        ResourceUnloaderTrigger trigger = gameObject.GetOrAddComponent<ResourceUnloaderTrigger>();
        trigger.AddAssetHandle(handle);

        return handle;
    }

    public static void LoadSprite(this Image image, string location)
    {
        AssetHandle assetHandle = GameEntry.Resource.LoadAssetAsync<Sprite>(location).Bind(image.gameObject);
        assetHandle.Completed += handle =>
        {
            image.sprite = handle.AssetObject as Sprite;
        };
    }
}