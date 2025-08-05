using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class ListVariable<T> : IListVariable<T>
    {
        public T this[int index] => _list[index];
        public int Count => _list.Count;
        public bool IsEmpty => _list.Count == 0;

        [SerializeField] protected List<T> _list = new List<T>();
        protected Action<IReadOnlyListVariable<T>> _onModified;

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public void Modifly(Action<List<T>> action)
        {
            action?.Invoke(_list);
            _onModified?.Invoke(this);
        }

        public IUnRegister Register(Action<IReadOnlyListVariable<T>> action, bool runInFirst = true)
        {
            _onModified += action;
            if (runInFirst)
            {
                action?.Invoke(this);
            }

            return new ListVariableUnRegister<T>(this, action);
        }

        public void UnRegister(Action<IReadOnlyListVariable<T>> action)
        {
            _onModified -= action;
        }

        public void UnRegisterAll()
        {
            _onModified = null;
        }
    }
}