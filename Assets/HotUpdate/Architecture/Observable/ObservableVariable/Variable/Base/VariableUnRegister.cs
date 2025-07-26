using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public class VariableUnRegister<TValue> : IUnRegister where TValue : IComparable<TValue>
    {
        public readonly IReadOnlyVariable<TValue> Variable;
        public readonly Action<TValue> Action;

        public VariableUnRegister(IReadOnlyVariable<TValue> variable, Action<TValue> action)
        {
            Variable = variable;
            Action = action;
        }

        public void UnRegister()
        {
            Variable?.UnRegister(Action);
        }
    }
}