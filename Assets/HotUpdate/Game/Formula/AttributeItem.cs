using System;
using System.Collections;
using System.Collections.Generic;
using Observable;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class AttributeItem : IAttribute
{
    [ShowInInspector, ReadOnly] public EAttributeKey Key { get; }
    [ShowInInspector] public long Max { get; set; } = int.MaxValue;
    [ShowInInspector] public long Min { get; set; } = int.MinValue;
    [ShowInInspector]
    public long Value
    {
        get => _value.Value;
        set => _value.Value = CombatUtility.Clamp(value, Min, Max);
    }
    public bool CanModifly { get; set; }

    private LongVariable _value = new LongVariable(0);

    public void Subscribe(Action<long> onValueChange, bool runInFirst = true)
    {
        _value.Register(onValueChange, runInFirst);
    }

    public void Unsubscribe(Action<long> onValueChange)
    {
        _value.UnRegister(onValueChange);
    }

    public void UnsubscribeAll()
    {
        _value.UnRegisterAll();
    }
}