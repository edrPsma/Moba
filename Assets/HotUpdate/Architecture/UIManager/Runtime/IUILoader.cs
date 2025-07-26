using System;
using UnityEngine;

namespace UI
{
    public interface IUILoader
    {
        void Load(string location, out object userData, Action<GameObject> loadOver);

        void LoadAsync(string location, out object userData, Action<GameObject> loadOver);

        void UnLoad(GameObject gameObject, object userData);
    }

}