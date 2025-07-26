using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.UI;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.Image, "SetColor")]
    [Serializable]
    public class SetImageColor : BaseBindImpact
    {
        public override UnityEngine.Object RecordObject => Image;

        public Image Image;
        public Color Color = new Color(1, 1, 1, 1);

        protected override void OnInvoke()
        {
            if (Image != null)
            {
                Image.color = Color;
            }
        }
    }
}