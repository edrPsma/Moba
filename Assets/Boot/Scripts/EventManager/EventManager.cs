using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    private interface IEventArgs { }

    private class EventArgs<T> : IEventArgs
    {
        public Action<T> Action;
    }

    public interface IUnregister
    {
        void Unregister();

        void Bind(GameObject gameObject);
    }

    public class Unregister<T> : IUnregister
    {
        public Action<T> Action;
        public EventManager Manager;

        public void Bind(GameObject gameObject)
        {
            if (gameObject.TryGetComponent(out UnregisterTrigger trigger))
            {
                trigger.Add(this);
            }
            else
            {
                gameObject.AddComponent<UnregisterTrigger>().Add(this);
            }
        }

        void IUnregister.Unregister()
        {
            Manager.UnregisterEvent(Action);
        }
    }

    public class UnregisterTrigger : MonoBehaviour
    {
        HashSet<IUnregister> _unregisters = new HashSet<IUnregister>();

        public void Add(IUnregister unregister)
        {
            _unregisters.Add(unregister);
        }

        void OnDestroy()
        {
            foreach (var item in _unregisters)
            {
                item.Unregister();
            }
        }
    }

    Dictionary<Type, IEventArgs> _eventListeners = new Dictionary<Type, IEventArgs>();

    public IUnregister RegisterEvent<T>(Action<T> listener)
    {
        Type type = typeof(T);
        if (!_eventListeners.ContainsKey(type))
        {
            _eventListeners[type] = new EventArgs<T>();
        }

        EventArgs<T> eventArgs = _eventListeners[type] as EventArgs<T>;
        eventArgs.Action += listener;

        IUnregister unregister = new Unregister<T>
        {
            Manager = this,
            Action = listener
        };
        return unregister;
    }

    public void UnregisterEvent<T>(Action<T> listener)
    {
        Type type = typeof(T);
        if (_eventListeners.ContainsKey(type))
        {
            EventArgs<T> eventArgs = _eventListeners[type] as EventArgs<T>;
            eventArgs.Action -= listener;
        }
    }

    public void TriggerEvent<T>(T args)
    {
        Type type = typeof(T);
        if (_eventListeners.ContainsKey(type))
        {
            EventArgs<T> eventArgs = _eventListeners[type] as EventArgs<T>;
            eventArgs.Action?.Invoke(args);
        }
    }

    public void TriggerEvent<T>() where T : new()
    {
        Type type = typeof(T);
        if (_eventListeners.ContainsKey(type))
        {
            EventArgs<T> eventArgs = _eventListeners[type] as EventArgs<T>;
            eventArgs.Action?.Invoke(new T());
        }
    }

    public void ClearEvents()
    {
        _eventListeners.Clear();
    }
}
