using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    public partial class BindComponent
    {
        Dictionary<string, int> _stateDic;
        public BindStateGroup[] StateDatas = new BindStateGroup[0];

        void BuildStateDic()
        {
            if (_stateDic != null) return;

            _stateDic = new Dictionary<string, int>();
            for (int i = 0; i < StateDatas.Length; i++)
            {
                if (_stateDic.ContainsKey(StateDatas[i].Name))
                {
                    Debug.LogError($"重复状态名, Name: {StateDatas[i].Name}", this);
                }
                else
                {
                    _stateDic.Add(StateDatas[i].Name, i);
                }
            }
        }
    }

    [System.Serializable]
    public class BindStateGroup
    {
        public string Name;

#if UNITY_EDITOR
        public string Desc;
#endif

        public BindStateData[] StateGroups;
    }

    [System.Serializable]
    public class BindStateData
    {
        public string ImpactType;
        [SerializeReference] public IBindImpact BindState;
    }
}