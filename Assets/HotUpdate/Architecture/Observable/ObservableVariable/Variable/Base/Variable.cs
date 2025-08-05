using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public abstract class Variable<TValue> : IReadOnlyVariable<TValue> where TValue : IComparable<TValue>
    {
        [SerializeField] protected TValue _value;

        Action<TValue> _onValueChange;

        public TValue Value
        {
            get => _value;
            set => SetValue(value);
        }

        public IUnRegister Register(Action<TValue> action, bool runInFirst = true)
        {
            _onValueChange += action;

            if (runInFirst)
            {
                action?.Invoke(_value);
            }

            return new VariableUnRegister<TValue>(this, action);
        }

        protected virtual void SetValue(TValue value)
        {
            if (_value?.CompareTo(value) != 0)
            {
                _onValueChange?.Invoke(value);
            }
            _value = value;
        }

        public void UnRegister(Action<TValue> action)
        {
            _onValueChange -= action;
        }

        public void UnRegisterAll()
        {
            _onValueChange = null;
        }
    }
}