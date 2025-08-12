using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Sirenix.OdinInspector;
using UnityEngine;

public class AttributeSet
{
    public const int HP_ID = 0;// 当前血量key值
    public const int Shield_ID = 2;// 当前护盾key值
    public FixInt HP => GetValue(HP_ID);
    public IAttribute HPAttribute => GetAttribute(HP_ID);
    public IAttribute ShieldAttribute => GetAttribute(Shield_ID);
    [ShowInInspector] Dictionary<int, IAttribute> _dic;
    LogicActor _actor;

    public void Initialize(LogicActor logicActor)
    {
        _actor = logicActor;
        _dic = new Dictionary<int, IAttribute>();
        foreach (var key in Enum.GetValues(typeof(EAttributeKey)))
        {
            _dic.Add((int)key, new AttributeItem());
        }
        _dic.Add(HP_ID, new AttributeItem());
        _dic.Add(Shield_ID, new AttributeItem());
    }

    public void ResetAttribute(int key, FixInt value, FixInt min, FixInt max)
    {
        if (!_dic.ContainsKey(key))
        {
            Debug.LogError($"Formula 属性不存在,key:{key}");
            return;
        }

        _dic[key].UnsubscribeAll();
        _dic[key].Min = min;
        _dic[key].Max = max;
        _dic[key].Value = value;
    }

    public void ResetAttribute(EAttributeKey key, FixInt value, FixInt min, FixInt max)
    {
        ResetAttribute((int)key, value, min, max);
    }

    public void SetValue(int key, FixInt value)
    {
        if (!_dic.ContainsKey(key))
        {
            Debug.LogError($"Formula 属性不存在,key:{key}");
            return;
        }

        _dic[key].Value = value;
    }

    public void SetValue(EAttributeKey key, FixInt value)
    {
        SetValue((int)key, value);
    }

    public void AddValue(EAttributeKey key, FixInt addValue)
    {
        AddValue((int)key, addValue);
    }

    public void AddValue(int key, FixInt addValue)
    {
        FixInt cur = GetValue(key);
        FixInt result = cur + addValue;
        SetValue(key, result);
    }

    public FixInt GetValue(int key)
    {
        if (!_dic.ContainsKey(key))
        {
            Debug.LogError($"Formula 属性不存在,key:{key}");
            return 0;
        }

        return _dic[key].Value;
    }

    public FixInt GetValue(EAttributeKey key)
    {
        return GetValue((int)key);
    }

    public IAttribute GetAttribute(int key)
    {
        if (!_dic.ContainsKey(key))
        {
            Debug.LogError($"Formula 属性不存在,key:{key}");
            return null;
        }

        return _dic[key];
    }

    public IAttribute GetAttribute(EAttributeKey key)
    {
        return GetAttribute((int)key);
    }

    public void OnReset()
    {
        foreach (var key in Enum.GetValues(typeof(EAttributeKey)))
        {
            ResetAttribute((EAttributeKey)key, 0, 0, int.MaxValue);
        }
        ResetAttribute(HP_ID, 0, 0, int.MaxValue);
        ResetAttribute(Shield_ID, 0, 0, int.MaxValue);
    }

    public void ResetHP(FixInt value, FixInt min, FixInt max)
    {
        HPAttribute.Min = min;
        HPAttribute.Max = max;
        HPAttribute.Value = FixIntMath.Clamp(value, min, max);
    }

    public void AddHP(LogicActor source, FixInt addValue)
    {
        AddValue(HP_ID, addValue);
        //TODO 触发造成伤害事件
        // if (addValue < 0 && source != null)
        // {
        //     _actor.EventOnTakeDamage?.Invoke(source, _actor, addValue);
        // }
    }

    public void AddShield(FixInt addValue) => AddValue(Shield_ID, addValue);
}
