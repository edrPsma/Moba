using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    public partial class BindComponent
    {
        Dictionary<string, int> _componentDic;
        public BindComponentData[] ComponentDatas = new BindComponentData[0];

        void BuildComponentDic()
        {
            if (_componentDic != null) return;

            _componentDic = new Dictionary<string, int>();
            for (int i = 0; i < ComponentDatas.Length; i++)
            {
                if (_componentDic.ContainsKey(ComponentDatas[i].Name))
                {
                    Debug.LogError($"重复组件名, Name: {ComponentDatas[i].Name}", this);
                }
                else
                {
                    _componentDic.Add(ComponentDatas[i].Name, i);
                }
            }
        }
    }

    [Serializable]
    public struct BindComponentData
    {
        public string Name;
        public string Type;
        public GameObject Reference;
        public UnityEngine.Object BindReference;
    }
}