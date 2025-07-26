using System;
using System.Collections.Generic;

namespace Observable
{
    [Serializable]
    public class TypeEventRegisteration<TData> : IRegisterations
    {
        public int Count => _set.Count;

        HashSet<Action<TData>> _set;
        Action<TData> _action;

        public TypeEventRegisteration()
        {
            _set = new HashSet<Action<TData>>();
        }

        public void Add(object action, object userData)
        {
            Action<TData> result = action as Action<TData>;
            _action += result;
            _set.Add(result);
        }

        public TAction GetAction<TAction>(object userData) where TAction : class
        {
            return _action as TAction;
        }

        public void Remove(object action, object userData)
        {
            Action<TData> result = action as Action<TData>;
            _action -= result;
            _set.Remove(result);
        }
    }
}