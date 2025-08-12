using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Observable;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AttributeItem : IAttribute
{
    [ShowInInspector, ReadOnly] public EAttributeKey Key { get; }
    [ShowInInspector] public FixInt Max { get; set; } = FixInt.MaxValue;
    [ShowInInspector] public FixInt Min { get; set; } = FixInt.MinValue;
    [ShowInInspector]
    public FixInt Value
    {
        get => _value.Value;
        set => _value.Value = FixIntMath.Clamp(value, Min, Max);
    }
    public bool CanModifly { get; set; }

    private FixIntVariable _value = new FixIntVariable(0);

    public void Subscribe(Action<FixInt> onValueChange, bool runInFirst = true)
    {
        _value.Register(onValueChange, runInFirst);
    }

    public void Unsubscribe(Action<FixInt> onValueChange)
    {
        _value.UnRegister(onValueChange);
    }

    public void UnsubscribeAll()
    {
        _value.UnRegisterAll();
    }
}