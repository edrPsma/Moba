using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public abstract class SkillExcutor : ISkillExcutor
{
    public bool Active { get; private set; }
    public SkillInfo SkillInfo { get; private set; }
    public FixIntVector3 Position { get; set; }
    public FixIntVector3 Direction { get; set; }
    public LogicActor LockTarget { get; set; }

    public List<LogicActor> Targets;//目标列表
    public List<EffectorItem> Effectors;
    public int DamageTargetNum;
    private ISelector _selector;
    private IFilter _filter;
    public IFilter Filter => _filter;

    #region 修正
    public int Coefficient;// 伤害系数
    public float Duration;// 持续时间
    public float Interval;// 间隔
    public float AttackTime;// 攻击总时间
    public float AttackTimer;// 攻击时间计时器
    public float DelayTimer;// 延迟时间计时器
    #endregion

    public abstract ISkillExcutor New();

    public void Start(SkillInfo skillInfo)
    {

    }

    public void Update(FixInt deltaTime)
    {

    }

    public void Dispose()
    {

    }
}
