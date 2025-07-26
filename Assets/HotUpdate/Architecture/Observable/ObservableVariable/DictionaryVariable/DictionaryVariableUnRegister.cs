using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public class DictionaryVariableUnRegister<K, V> : IUnRegister
    {
        public readonly IDictionaryVariable<K, V> Variable;
        public readonly Action<IReadOnlyDictionaryVariable<K, V>> Action;

        public DictionaryVariableUnRegister(IDictionaryVariable<K, V> variable, Action<IReadOnlyDictionaryVariable<K, V>> action)
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