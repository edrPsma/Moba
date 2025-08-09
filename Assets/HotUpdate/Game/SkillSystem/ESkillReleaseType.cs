using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESkillReleaseType
{
    /// <summary>
    /// 无目标
    /// </summary>
    NoTarget = 0,

    /// <summary>
    /// 指定单位
    /// </summary>
    TargetUnit = 1,

    /// <summary>
    /// 指定目标点
    /// </summary>
    TargetPoint = 2,

    /// <summary>
    /// 指定方向
    /// </summary>
    VectorSkill = 3,
}
