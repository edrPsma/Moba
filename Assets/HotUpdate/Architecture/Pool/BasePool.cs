using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Pool
{
    public abstract class BasePool : IPool
    {
        [ShowInInspector, ReadOnly] public EPoolType PoolType { get; protected set; }
        [ShowInInspector, ReadOnly] public int Capacity { get; protected set; }
        [ShowInInspector, ReadOnly] public int Used { get; protected set; }
        [ShowInInspector, ReadOnly] public int Free { get; protected set; }
        protected Queue<object> _pool;

        public BasePool() { }

        public BasePool(EPoolType poolType, int capacity)
        {
            _pool = new Queue<object>();
            PoolType = poolType;
            Capacity = capacity;
            Free = capacity;
            Used = 0;

            for (int i = 0; i < capacity; i++)
            {
                _pool.Enqueue((this as IPool).Create());
            }
        }

        public void Release(object obj)
        {
            if (obj == null) return;

            OnRelease(obj);

            if (Free < Capacity)
            {
                _pool.Enqueue(obj);
                Free++;
                Used--;
            }
        }

        public object Spawn()
        {
            object result = null;
            switch (PoolType)
            {
                case EPoolType.Scalable: result = SpawnScalable(); break;
                case EPoolType.Fixed: result = SpawnFixed(); break;
            }

            OnSpawn(result);

            return result;
        }

        object SpawnFixed()
        {
            object result = null;
            if (Free > 0)
            {
                result = _pool.Dequeue();
                Used++;
                Free--;
            }

            return result;
        }

        object SpawnScalable()
        {
            object result;
            if (Free <= 0)
            {
                result = CreateNew();
                Capacity++;
                Used++;
            }
            else
            {
                result = _pool.Dequeue();
                Used++;
                Free--;
            }

            return result;
        }

        object IPool.Create()
        {
            return CreateNew();
        }

        public void Dispose()
        {
            _pool = null;
            Capacity = 0;
            Free = 0;
            Used = 0;
            OnDispose();
        }

        protected virtual void OnRelease(object obj) { }
        protected abstract void OnSpawn(object obj);
        protected abstract object CreateNew();
        protected abstract void OnDispose();
    }

}
