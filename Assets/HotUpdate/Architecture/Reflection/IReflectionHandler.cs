using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReflectionHandler
{
    Dictionary<Type, Dictionary<object, object>> RefDic { get; }
    Dictionary<Type, Dictionary<object, Delegate>> DelegateDic { get; }

    void Register();
}
