using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.Text, "SetText")]
    [Serializable]
    public class TextSetText : BaseBindImpact
    {
        public override UnityEngine.Object RecordObject => Text;

        public Text Text;
        public string Value;

        protected override void OnInvoke()
        {
            if (Text != null)
            {
                Text.text = Value;
            }
        }
    }
}