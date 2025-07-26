using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    public partial class BindComponent
    {
        Dictionary<string, int> _arrayDic;
        public BindArrayData[] ArrayDatas = new BindArrayData[0];

        void BuildArrayDic()
        {
            if (_arrayDic != null) return;

            _arrayDic = new Dictionary<string, int>();
            for (int i = 0; i < ArrayDatas.Length; i++)
            {
                if (_arrayDic.ContainsKey(ArrayDatas[i].Name))
                {
                    Debug.LogError($"重复数组名, Name: {ArrayDatas[i].Name}", this);
                }
                else
                {
                    _arrayDic.Add(ArrayDatas[i].Name, i);
                }
            }
        }
    }

    [Serializable]
    public struct BindArrayData
    {
        public string Name;
        public string Type;
        public GameObject[] Reference;
        public UnityEngine.Object[] BindReference;
    }
}