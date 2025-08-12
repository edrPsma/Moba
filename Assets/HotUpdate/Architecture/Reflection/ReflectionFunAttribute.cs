using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class ReflectionFunAttribute : Attribute
{
    public Type DelegateType;
    public object Key;

    public ReflectionFunAttribute(Type delegateType, object key)
    {
        DelegateType = delegateType;
        Key = key;
    }

}
