using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EEffectType
{
    /// <summary>
    /// 造成伤害，默认类型，每个技能必有
    /// </summary>
    Damage = 0,

    /// <summary>
    /// 添加Buff
    /// </summary>
    AddBuff = 1,
}