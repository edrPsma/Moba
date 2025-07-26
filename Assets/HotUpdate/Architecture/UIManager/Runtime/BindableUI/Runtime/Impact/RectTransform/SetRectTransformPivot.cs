using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.RectTransform, "SetPivot")]
    [Serializable]
    public class SetRectTransformPivot : BaseBindImpact
    {
        public override UnityEngine.Object RecordObject => RectTransform;

        public RectTransform RectTransform;
        public Vector2 Pivot;

        protected override void OnInvoke()
        {
            if (RectTransform == null) return;

            RectTransform.pivot = Pivot;
        }
    }
}