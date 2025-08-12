using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Template;
using System;
using System.Reflection;
using System.Linq;
using Sirenix.OdinInspector;

namespace Reflection
{
    public partial class ReflectionManager : MonoSingleton<IReflectionManager, ReflectionManager>, IReflectionManager
    {
        [ShowInInspector] Dictionary<Type, Dictionary<object, object>> _refDic => _handler.RefDic;
        [ShowInInspector] Dictionary<Type, Dictionary<object, Delegate>> _delegateDic => _handler.DelegateDic;
        IReflectionHandler _handler;

        protected override void OnInit()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Init(IReflectionHandler handler)
        {
            _handler = handler;
            handler.Register();
        }

        public T Get<T>(object key) where T : class
        {
            Type type = typeof(T);
            if (_refDic.ContainsKey(type))
            {
                if (_refDic[type].ContainsKey(key))
                {
                    return _refDic[type][key] as T;
                }
            }

            return null;
        }

        public T2 Get<T, T2>(object key) where T2 : class
        {
            Type type = typeof(T);
            if (_refDic.ContainsKey(type))
            {
                if (_refDic[type].ContainsKey(key))
                {
                    return _refDic[type][key] as T2;
                }
            }

            return null;
        }

        public Dictionary<object, object> GetDic<T>() where T : class
        {
            Type type = typeof(T);
            return _refDic.GetValue(type) ?? new Dictionary<object, object>();
        }

        public Action GetAction(Type baseType, object key)
        {
            return _delegateDic.GetValue(baseType)?.GetValue(key) as Action;
        }

        public Action<T> GetAction<T>(Type baseType, object key)
        {
            return _delegateDic.GetValue(baseType)?.GetValue(key) as Action<T>;
        }

        public Action<T1, T2> GetAction<T1, T2>(Type baseType, object key)
        {
            return _delegateDic.GetValue(baseType)?.GetValue(key) as Action<T1, T2>;
        }

        public Action<T1, T2, T3> GetAction<T1, T2, T3>(Type baseType, object key)
        {
            return _delegateDic.GetValue(baseType)?.GetValue(key) as Action<T1, T2, T3>;
        }

        public Func<T> GetFunc<T>(Type baseType, object key)
        {
            return _delegateDic.GetValue(baseType)?.GetValue(key) as Func<T>;
        }

        public Func<T, T2> GetFunc<T, T2>(Type baseType, object key)
        {
            return _delegateDic.GetValue(baseType)?.GetValue(key) as Func<T, T2>;
        }

        public Func<T, T1, T2> GetFunc<T, T1, T2>(Type baseType, object key)
        {
            return _delegateDic.GetValue(baseType)?.GetValue(key) as Func<T, T1, T2>;
        }

        public Func<T, T1, T2, T3> GetFunc<T, T1, T2, T3>(Type baseType, object key)
        {
            return _delegateDic.GetValue(baseType)?.GetValue(key) as Func<T, T1, T2, T3>;
        }
    }
}