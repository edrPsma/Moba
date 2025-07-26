using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    [Serializable]
    public class TypeEventUnRegister<TData> : TypeEventUnRegister<string, TData>
    {
        public TypeEventUnRegister(TypeEventSource<string> source, string eventName, Action<TData> action) : base(source, eventName, action) { }
    }

    [Serializable]
    public class TypeEventUnRegister<TEventName, TData> : IUnRegister
    {
        /// <summary>
        /// 事件接口
        /// </summary>
        public readonly TypeEventSource<TEventName> Source;

        /// <summary>
        /// 回调委托
        /// </summary>
        public readonly Action<TData> Action;

        /// <summary>
        /// 事件名
        /// </summary>
        public readonly TEventName EventName;

        public TypeEventUnRegister(TypeEventSource<TEventName> source, TEventName eventName, Action<TData> action)
        {
            Source = source;
            EventName = eventName;
            Action = action;
        }

        void IUnRegister.UnRegister()
        {
            Source?.UnRegister(EventName, Action);
        }
    }
}