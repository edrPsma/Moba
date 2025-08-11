using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct DamageInfo
{
    /// <summary>
    /// 伤害信息是否有效的
    /// </summary>
    public bool IsValid;

    /// <summary>
    /// 是否是技能
    /// </summary>
    public bool IsSkill;

    /// <summary>
    /// 是否暴击
    /// </summary>
    public bool IsCriticalStrike;

    /// <summary>
    /// 最终伤害数值
    /// </summary>
    public long FinalDamageValue;

    /// <summary>
    /// 伤害数值
    /// </summary>
    public long DamageValue;

    /// <summary>
    /// 其他伤害数值
    /// </summary>
    public long OtherValue;
}