using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public interface IReadOnlyListVariable<T> : IReadOnlyList<T>
    {
        /// <summary>
        /// 列表是否为空
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        /// 注册列表修改事件
        /// </summary>
        /// <param name="action">列表修改事件</param>
        /// <param name="runInFirst">注册时是否调用一次</param>
        /// <returns></returns>
        IUnRegister Register(Action<IReadOnlyListVariable<T>> action, bool runInFirst);

        /// <summary>
        /// 取消注册列表修改事件
        /// </summary>
        /// <param name="action">列表修改事件</param>
        void UnRegister(Action<IReadOnlyListVariable<T>> action);

        /// <summary>
        /// 取消注册所有列表修改事件
        /// </summary>
        void UnRegisterAll();
    }
}