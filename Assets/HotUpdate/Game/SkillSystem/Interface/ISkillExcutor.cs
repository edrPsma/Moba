using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public interface ISkillExcutor
{
    bool Active { get; }

    /// <summary>
    /// 技能信息
    /// </summary>
    SkillInfo SkillInfo { get; }

    /// <summary>
    /// 位置
    /// </summary>
    FixIntVector3 Position { get; set; }

    /// <summary>
    /// 方向 z轴
    /// </summary>
    FixIntVector3 Direction { get; set; }

    /// <summary>
    /// 锁定的目标，可能为空
    /// </summary>
    LogicActor LockTarget { get; set; }

    /// <summary>
    /// new 方法
    /// </summary>
    /// <returns></returns>
    ISkillExcutor New();

    /// <summary>
    /// 初始化
    /// </summary>
    void Start(SkillInfo skillInfo);

    /// <summary>
    /// 逻辑帧更新
    /// </summary>
    void Update(FixInt deltaTime);

    /// <summary>
    /// 释放
    /// </summary>
    void Dispose();
}