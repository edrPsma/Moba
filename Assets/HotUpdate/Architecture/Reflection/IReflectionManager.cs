using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IReflectionManager
{
    void Init(IReflectionHandler handler);
    T Get<T>(object key) where T : class;
    T2 Get<T, T2>(object key) where T2 : class;
    Dictionary<object, object> GetDic<T>() where T : class;

    Action GetAction(Type baseType, object key);
    Action<T> GetAction<T>(Type baseType, object key);
    Action<T1, T2> GetAction<T1, T2>(Type baseType, object key);
    Action<T1, T2, T3> GetAction<T1, T2, T3>(Type baseType, object key);
    Func<T> GetFunc<T>(Type baseType, object key);
    Func<T, T2> GetFunc<T, T2>(Type baseType, object key);
    Func<T, T1, T2> GetFunc<T, T1, T2>(Type baseType, object key);
    Func<T, T1, T2, T3> GetFunc<T, T1, T2, T3>(Type baseType, object key);
}
