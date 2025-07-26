using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.RectTransform, "SetAnchor")]
    [Serializable]
    public class SetRectTransformAnchor : BaseBindImpact
    {
        public override UnityEngine.Object RecordObject => throw new NotImplementedException();

        public RectTransform RectTransform;
        [Range(0, 1)] public float MinX;
        [Range(0, 1)] public float MinY;
        [Range(0, 1)] public float MaxX;
        [Range(0, 1)] public float MaxY;

        protected override void OnInvoke()
        {
            if (RectTransform == null) return;

            RectTransform.anchorMin = new Vector2(MinX, MinY);
            RectTransform.anchorMax = new Vector2(MaxX, MaxY);
        }
    }
}