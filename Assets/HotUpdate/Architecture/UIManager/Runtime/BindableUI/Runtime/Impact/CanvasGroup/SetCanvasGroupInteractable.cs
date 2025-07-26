using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.CanvasGroup, "SetInteractable")]
    public class SetCanvasGroupInteractable : BaseBindImpact
    {
        public override Object RecordObject => CanvasGroup;
        public CanvasGroup CanvasGroup;
        public bool Interactable;

        protected override void OnInvoke()
        {
            if (CanvasGroup == null) return;

            CanvasGroup.interactable = Interactable;
        }
    }
}