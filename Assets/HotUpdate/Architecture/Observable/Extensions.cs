using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Observable
{
    public static partial class Extensions
    {
        public static IUnRegister Observe<T>(this Text text, IReadOnlyVariable<T> variable) where T : IComparable<T>
        {
            return variable.Register(value => text.text = value.ToString(), true);
        }

        public static IUnRegister Observe<T>(this Text text, IReadOnlyVariable<T> variable, string format) where T : IComparable<T>
        {
            return variable.Register(value => text.text = string.Format(format, variable.Value), true);
        }

        public static IUnRegister Observe(this GameObject gameObject, BoolVariable variable)
        {
            return variable.Register(value => gameObject.SetActive(value), true);
        }

        public static IUnRegister Observe(this Slider slider, FloatVariable variable)
        {
            return variable.Register(value => slider.value = variable.Value, true);
        }

        public static IUnRegister Observe(this CanvasGroup canvasGroup, BoolVariable variable)
        {
            return variable.Register(value =>
            {
                if (variable.Value)
                {
                    canvasGroup.alpha = 1;
                    canvasGroup.blocksRaycasts = true;
                }
                else
                {
                    canvasGroup.alpha = 0;
                    canvasGroup.blocksRaycasts = false;
                }
            }, true);
        }
    }
}