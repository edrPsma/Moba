using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.RectTransform, "SetAnchorPosition")]
    [Serializable]
    public class SetRectTransformAnchorPosition : BaseBindImpact
    {
        public override UnityEngine.Object RecordObject => RectTransform;

        public RectTransform RectTransform;
        public Vector2 AnchorPosition;

        protected override void OnInvoke()
        {
            if (RectTransform == null) return;

            RectTransform.anchoredPosition = AnchorPosition;
        }
    }
}