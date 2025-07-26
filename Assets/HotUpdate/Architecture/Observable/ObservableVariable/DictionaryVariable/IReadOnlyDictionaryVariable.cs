using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public interface IReadOnlyDictionaryVariable<K, V> : IReadOnlyDictionary<K, V>
    {
        /// <summary>
        /// 该字典是否为空
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 注册字典修改事件
        /// </summary>
        /// <param name="action">字典修改事件</param>
        /// <param name="runInFirst">注册时是否调用一次</param>
        /// <returns></returns>
        IUnRegister Register(Action<IReadOnlyDictionaryVariable<K, V>> action, bool runInFirst);

        /// <summary>
        /// 取消注册字典修改事件
        /// </summary>
        /// <param name="action"></param>
        void UnRegister(Action<IReadOnlyDictionaryVariable<K, V>> action);

        /// <summary>
        /// 取消注册所有字典修改事件
        /// </summary>
        void UnRegisterAll();
    }
}