using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.CanvasGroup, "SetAlpha")]
    public class SetCanvasGroupAlpha : BaseBindImpact
    {
        public override Object RecordObject => CanvasGroup;

        public CanvasGroup CanvasGroup;
        [Range(0, 1f)] public float Alpha;

        protected override void OnInvoke()
        {
            if (CanvasGroup == null) return;

            CanvasGroup.alpha = Alpha;
        }
    }
}
