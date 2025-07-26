using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.Text, "SetColor")]
    [Serializable]
    public class TextSetColor : BaseBindImpact
    {
        public override UnityEngine.Object RecordObject => Text;

        public Text Text;
        public Color Color = new Color(1, 1, 1, 1);

        protected override void OnInvoke()
        {
            if (Text != null)
            {
                Text.color = Color;
            }
        }
    }
}