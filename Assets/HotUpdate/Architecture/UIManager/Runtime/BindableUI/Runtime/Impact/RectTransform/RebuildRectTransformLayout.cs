using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.RectTransform, "ForceRebuildLayout")]
    [Serializable]
    public class RebuildRectTransformLayout : BaseBindImpact
    {
        public override UnityEngine.Object RecordObject => RectTransform;

        public RectTransform RectTransform;

        protected override void OnInvoke()
        {
            if (RectTransform == null) return;

            foreach (var group in RectTransform.GetComponentsInChildren<LayoutGroup>())
            {
                LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)group.transform);
            }
            LayoutRebuilder.ForceRebuildLayoutImmediate(RectTransform);
        }
    }
}