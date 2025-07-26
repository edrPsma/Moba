using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public class ListVariableUnRegister<T> : IUnRegister
    {
        public readonly IListVariable<T> Variable;
        public readonly Action<IReadOnlyListVariable<T>> Action;

        public ListVariableUnRegister(IListVariable<T> variable, Action<IReadOnlyListVariable<T>> action)
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