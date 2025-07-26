using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BindableUI.Runtime
{
    public abstract class BaseBindImpact : IBindImpact
    {
        public abstract UnityEngine.Object RecordObject { get; }

        void IBindImpact.Invoke()
        {
#if UNITY_EDITOR
            if (RecordObject != null)
            {
                UnityEditor.Undo.RecordObject(RecordObject, "BindableUI.Impact");
            }
#endif
            OnInvoke();
        }

        protected virtual void OnInvoke()
        {

        }
    }
}