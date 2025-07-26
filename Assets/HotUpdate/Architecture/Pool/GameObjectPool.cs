using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pool
{
    public delegate void DisposeCloneObject<T>(T clone);
    public class GameObjectPool<T> : BasePool where T : UnityEngine.Object
    {
        public event OnCreate<T> OnCreateEvent;
        public event OnGet<T> OnGetEvent;
        public event OnRelease<T> OnReleaseEvent;
        T _clone;
        DisposeCloneObject<T> _disposeFun;
        public GameObjectPool(EPoolType poolType, int capacity, T clone, OnCreate<T> onCreate, DisposeCloneObject<T> disposeFun)
        {
            _clone = clone;
            _disposeFun = disposeFun;

            _pool = new Queue<object>();
            PoolType = poolType;
            Capacity = capacity;
            Free = capacity;
            Used = 0;
            OnCreateEvent += onCreate;

            for (int i = 0; i < capacity; i++)
            {
                _pool.Enqueue((this as IPool).Create());
            }
        }

        protected override object CreateNew()
        {
            T result = GameObject.Instantiate(_clone);
            OnCreateEvent?.Invoke(result);
            return result;
        }

        protected override void OnDispose()
        {
            OnCreateEvent = null;
            OnGetEvent = null;
            OnReleaseEvent = null;
            _disposeFun?.Invoke(_clone);
            _clone = null;
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

