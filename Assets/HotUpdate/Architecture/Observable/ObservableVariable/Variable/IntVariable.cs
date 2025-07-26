using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class IntVariable : RangeVariable<int>
    {

        public IntVariable() : base(0, int.MinValue, int.MaxValue)
        {
            _min = int.MinValue;
            _max = int.MaxValue;
        }

        public IntVariable(int value) : base(value, int.MinValue, int.MaxValue)
        {
            _min = int.MinValue;
            _max = int.MaxValue;
        }

        public IntVariable(int value, int min, int max) : base(value, min, max) { }
    }
}