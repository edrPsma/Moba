using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.GameObject, "SetActive")]
    public class SetGameObjectActive : BaseBindImpact
    {
        public override Object RecordObject => GameObject;

        public GameObject GameObject;
        public bool Active;

        protected override void OnInvoke()
        {
            GameObject?.SetActive(Active);
        }
    }
}