using System;
using System.Collections;
using System.Collections.Generic;

namespace Observable
{
    public interface IEventSource<TEventName>
    {
        IUnRegister Register<TData>(TEventName eventName, Action<TData> action);

        void UnRegister<TData>(TEventName eventName, Action<TData> action);

        void UnRegisterAll(TEventName eventName);

        void Trigger<TData>(TEventName eventName, TData userData);

        void Clear();
    }
}