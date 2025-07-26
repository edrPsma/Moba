using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public abstract class RangeVariable<TValue> : Variable<TValue> where TValue : IComparable<TValue>
    {
        public TValue Min
        {
            get => _min;
            set => SetMin(value);
        }

        public TValue Max
        {
            get => _max;
            set => SetMax(value);
        }

        [SerializeField] protected TValue _min;
        [SerializeField] protected TValue _max;

        public RangeVariable(TValue value, TValue min, TValue max)
        {
            _min = min;
            _max = max;
            _value = Clamp(value, _min, _max);
        }


        protected override void SetValue(TValue value)
        {
            value = Clamp(value, _min, _max);
            base.SetValue(value);
        }

        protected virtual TValue Clamp(TValue value, TValue min, TValue max)
        {
            if (value?.CompareTo(min) < 0)
            {
                value = _min;
            }
            if (value?.CompareTo(max) > 0)
            {
                value = _max;
            }

            return value;
        }

        protected virtual void SetMin(TValue value)
        {
            _min = value;
            SetValue(_value);
        }

        protected virtual void SetMax(TValue value)
        {
            _max = value;
            SetValue(_value);
        }
    }
}