using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class StringVariable : Variable<string>
    {
        public StringVariable()
        {
            _value = string.Empty;
        }

        public StringVariable(string value)
        {
            _value = value;
        }
    }
}