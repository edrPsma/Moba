using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    public static HandleBase AddAssetHandle(this HandleBase handle, GameObject gameObject)
    {
        ResourceUnloaderTrigger trigger = gameObject.GetOrAddComponent<ResourceUnloaderTrigger>();
        trigger.AddAssetHandle(handle);

        return handle;
    }
}