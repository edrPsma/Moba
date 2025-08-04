using System;
using System.Collections;
using System.Collections.Generic;

namespace Observable
{
    [Serializable]
    public class TypeEventSource : TypeEventSource<string>
    {
        public IUnRegister Register<TData>(Action<TData> action)
        {
            string eventName = typeof(TData).Name;

            return Register(eventName, action);
        }

        public void Trigger<TData>() where TData : new()
        {
            string eventName = typeof(TData).Name;
            TData data = new TData();

            Trigger(eventName, data);
        }

        public void Trigger<TData>(TData userData)
        {
            string eventName = typeof(TData).Name;

            Trigger(eventName, userData);
        }

        public void UnRegister<TData>(Action<TData> action)
        {
            string eventName = typeof(TData).Name;

            UnRegister(eventName, action);
        }
    }

    public class TypeEventSource<TEventName> : IEventSource<TEventName>
    {
        Dictionary<TEventName, IRegisterations> _eventDir;

        public TypeEventSource()
        {
            _eventDir = new Dictionary<TEventName, IRegisterations>();
        }

        public IUnRegister Register<TData>(TEventName eventName, Action<TData> action)
        {
            IRegisterations registerations;

            if (!_eventDir.TryGetValue(eventName, out registerations))
            {
                registerations = new TypeEventRegisteration<TData>();
                _eventDir.Add(eventName, registerations);
            }

            if (registerations == null)
            {
                throw new DuplicateEventNameException(eventName.ToString());
            }
            registerations.Add(action, null);

            return new TypeEventUnRegister<TEventName, TData>(this, eventName, action);
        }

        public void Trigger<TData>(TEventName eventName, TData userData)
        {
            IRegisterations registerations;
            if (_eventDir.TryGetValue(eventName, out registerations))
            {
                TypeEventRegisteration<TData> registeration = registerations as TypeEventRegisteration<TData>;

                Action<TData> action = registeration.GetAction<Action<TData>>(null);
                action?.Invoke(userData);
            }
        }

        public void UnRegister<TData>(TEventName eventName, Action<TData> action)
        {
            IRegisterations registerations;

            if (_eventDir.TryGetValue(eventName, out registerations))
            {
                registerations.Remove(action, null);

                if (registerations.Count <= 0)
                {
                    _eventDir.Remove(eventName);
                }
            }
        }

        public void UnRegisterAll(TEventName eventName)
        {
            _eventDir.Remove(eventName);
        }

        public void Clear()
        {
            _eventDir.Clear();
        }
    }
}