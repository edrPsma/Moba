using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace BindableUI.Runtime
{
    [BindImpact(EImpactGroup.Image, "ChangeSprite")]
    [Serializable]
    public class ChangeImageSprite : BaseBindImpact
    {
        public override UnityEngine.Object RecordObject => Image;

        public Image Image;
        public Sprite Sprite;
        public bool SetNativeSize;

        protected override void OnInvoke()
        {
            if (Image != null)
            {
                Image.sprite = Sprite;
                if (SetNativeSize)
                {
                    Image.SetNativeSize();
                }
            }
        }
    }
}