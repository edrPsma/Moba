using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class BoolVariable : Variable<bool>
    {
        public BoolVariable()
        {
            _value = false;
        }

        public BoolVariable(bool value)
        {
            _value = value;
        }
    }
}