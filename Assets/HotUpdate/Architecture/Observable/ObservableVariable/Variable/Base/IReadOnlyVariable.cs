using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public interface IReadOnlyVariable<TValue> where TValue : IComparable<TValue>
    {
        /// <summary>
        /// 值
        /// </summary>
        TValue Value { get; }

        /// <summary>
        /// 注册值变化事件
        /// </summary>
        /// <param name="action">值变化事件</param>
        /// <param name="runInFirst">注册时是否调用一次</param>
        /// <returns></returns>
        IUnRegister Register(Action<TValue> action, bool runInFirst);

        /// <summary>
        /// 取消注册值变化事件
        /// </summary>
        /// <param name="action">值变化事件</param>
        void UnRegister(Action<TValue> action);

        /// <summary>
        /// 取消注册所有值变化事件
        /// </summary>
        void UnRegisterAll();
    }
}