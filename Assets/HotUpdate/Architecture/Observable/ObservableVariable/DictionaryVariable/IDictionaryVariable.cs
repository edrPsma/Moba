using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public interface IDictionaryVariable<K, V> : IReadOnlyDictionaryVariable<K, V>
    {
        /// <summary>
        /// 修改字典
        /// </summary>
        /// <param name="action"></param>
        void Modifly(Action<Dictionary<K, V>> action);
    }
}