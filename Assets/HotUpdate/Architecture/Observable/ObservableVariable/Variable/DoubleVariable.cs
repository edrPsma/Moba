using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class DoubleVariable : RangeVariable<double>
    {
        public DoubleVariable() : base(0, double.MinValue, double.MaxValue)
        {
            _min = double.MinValue;
            _max = double.MaxValue;
        }

        public DoubleVariable(double value) : base(value, double.MinValue, double.MaxValue)
        {
            _min = double.MinValue;
            _max = double.MaxValue;
        }

        public DoubleVariable(double value, double min, double max) : base(value, min, max) { }
    }
}