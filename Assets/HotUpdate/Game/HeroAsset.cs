using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroAsset", menuName = "HeroAsset")]
public class HeroAsset : ScriptableObject
{
    [SerializeField] AssetItem[] _assets;

    public T Get<T>(string name) where T : Object
    {
        for (int i = 0; i < _assets.Length; i++)
        {
            if (_assets[i].Name == name)
            {
                return _assets[i].Asset as T;
            }
        }

        return null;
    }
}

[System.Serializable]
public class AssetItem
{
    public string Name;
    public Object Asset;
}