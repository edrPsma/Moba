using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class FloatVariable : RangeVariable<float>
    {
        public FloatVariable() : base(0, float.MinValue, float.MaxValue)
        {
            _min = float.MinValue;
            _max = float.MaxValue;
        }

        public FloatVariable(float value) : base(value, float.MinValue, float.MaxValue)
        {
            _min = float.MinValue;
            _max = float.MaxValue;
        }

        public FloatVariable(float value, float min, float max) : base(value, min, max) { }
    }
}