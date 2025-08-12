using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class EffectorItem
{
    public int EffectId;
    public ETriggerTiming TriggerTiming;
    public int Type;
    public ISkillEffector Effector;
    [ShowInInspector] public List<int> EffectValue;

    public EffectorItem()
    {
        EffectValue = new List<int>();
    }

    public void OnReset()
    {
        EffectId = 0;
        Type = -1;
        EffectValue.Clear();
        Effector = null;
    }
}

public enum ETriggerTiming
{
    /// <summary>
    /// 释放技能时
    /// </summary>
    OnRelease = 0,

    /// <summary>
    /// 计算伤害前
    /// </summary>
    BeforeCalDamage = 1,

    /// <summary>
    /// 计算伤害后
    /// </summary>
    AfterCalDamage = 2,
}