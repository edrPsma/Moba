using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pool
{
    public delegate void OnCreate<T>(T obj);
    public delegate void OnGet<T>(T obj);
    public delegate void OnRelease<T>(T obj);

    [System.Serializable]
    public class ObjectPool<T> : BasePool where T : class, new()
    {
        public event OnCreate<T> OnCreateEvent;
        public event OnGet<T> OnGetEvent;
        public event OnRelease<T> OnReleaseEvent;

        public ObjectPool(EPoolType poolType, int capacity) : base(poolType, capacity) { }

        protected override object CreateNew()
        {
            T result = new T();
            OnCreateEvent?.Invoke(result);
            return result;
        }

        protected override void OnDispose()
        {
            OnCreateEvent = null;
            OnGetEvent = null;
            OnReleaseEvent = null;
        }

        protected override void OnSpawn(object obj)
        {
            OnGetEvent?.Invoke(obj as T);
        }

        protected override void OnRelease(object obj)
        {
            OnReleaseEvent?.Invoke(obj as T);
        }

        public T SpawnByType()
        {
            return Spawn() as T;
        }
    }
}

