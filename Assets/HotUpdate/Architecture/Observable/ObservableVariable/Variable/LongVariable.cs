using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class LongVariable : RangeVariable<long>
    {
        public LongVariable() : base(0, long.MinValue, long.MaxValue)
        {
            _min = long.MinValue;
            _max = long.MaxValue;
        }

        public LongVariable(long value) : base(value, long.MinValue, long.MaxValue)
        {
            _min = long.MinValue;
            _max = long.MaxValue;
        }

        public LongVariable(long value, long min, long max) : base(value, min, max) { }
    }
}