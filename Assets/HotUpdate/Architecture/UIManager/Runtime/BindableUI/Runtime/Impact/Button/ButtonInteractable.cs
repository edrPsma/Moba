using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.Button, "SetInteractable")]
    public class ButtonInteractable : BaseBindImpact
    {
        public override Object RecordObject => Button;

        public Button Button;
        public bool Interactable;

        protected override void OnInvoke()
        {
            if (Button == null) return;

            Button.interactable = Interactable;
        }
    }
}