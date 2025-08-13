using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroAsset", menuName = "HeroAsset")]
public class HeroAsset : ScriptableObject
{
    public AssetItem[] Assets;
}

[System.Serializable]
public class AssetItem
{
    [HideInInspector] public string Path;
    [OnValueChanged(nameof(OnAssetChange))] public Object Asset;

    void OnAssetChange()
    {
#if UNITY_EDITOR
        string path = UnityEditor.AssetDatabase.GetAssetPath(Asset);
        Path = path;
#endif
    }
}