using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BindableUI.Runtime
{
    public partial class BindComponent : MonoBehaviour
    {
        public T Get<T>(string name) where T : UnityEngine.Object
        {
            BuildComponentDic();

            if (_componentDic.TryGetValue(name, out int index))
            {
                // GameObject类型特殊处理
                if (typeof(T) == typeof(GameObject))
                {
                    return ComponentDatas[index].Reference as T;
                }
                else if (ComponentDatas[index].BindReference is T)
                {
                    return ComponentDatas[index].BindReference as T;
                }
                else
                {
                    return ComponentDatas[index].Reference.GetComponent<T>();
                }
            }

            return null;
        }

        public T[] GetArray<T>(string name) where T : UnityEngine.Object
        {
            BuildArrayDic();

            if (_arrayDic.TryGetValue(name, out int index))
            {
                // GameObject类型特殊处理
                if (typeof(T) == typeof(GameObject))
                {
                    return ArrayDatas[index].Reference as T[];
                }
                else
                {
                    return ArrayDatas[index].BindReference.OfType<T>().ToArray();
                }
            }

            return null;
        }

        public T GetAsset<T>(string name) where T : UnityEngine.Object
        {
            BuildAssetDic();

            if (_assetDic.TryGetValue(name, out int index))
            {
                return AssetDatas[index].BindReference as T;
            }

            return null;
        }

        public void ChangeState(string states)
        {
            BuildStateDic();

            string[] list = states.Split('|');
            for (int i = 0; i < list.Length; i++)
            {
                if (_stateDic.ContainsKey(list[i]))
                {
                    int index = _stateDic[list[i]];
                    foreach (var item in StateDatas[index].StateGroups)
                    {
                        item.BindState.Invoke();
                    }
                }
                else
                {
                    Debug.LogError($"不存在该状态, Name: {list[i]}");
                }
            }
        }
    }
}