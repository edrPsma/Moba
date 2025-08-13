using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using Zenject;

public abstract class SkillExcutor : ISkillExcutor
{
    [Inject] public IDamageMarkFactory DamageMarkFactory;
    [Inject] public ISkillSystem SkillSystem;

    public bool Active { get; private set; }
    public SkillInfo SkillInfo { get; private set; }
    public FixIntVector3 Position { get; set; }
    public FixIntVector3 Direction { get; set; }
    public LogicActor LockTarget { get; set; }
    public bool DamageStart { get; private set; }

    public List<LogicActor> Targets;//目标列表
    public EffectorItem[] Effectors;
    public int DamageTargetNum;
    private ISelector _selector;
    private IFilter _filter;
    public IFilter Filter => _filter;

    #region 修正
    public FixInt Coefficient;// 伤害系数
    public FixInt Duration;// 持续时间
    public FixInt Interval;// 间隔
    public FixInt AttackTime;// 攻击总时间
    public FixInt AttackTimer;// 攻击时间计时器
    public FixInt DelayTimer;// 延迟时间计时器
    #endregion

    public abstract ISkillExcutor New();

    void ISkillExcutor.Start(SkillInfo skillInfo)
    {
        MVCContainer.Inject(this);
        Active = true;
        SkillInfo = skillInfo;
        AttackTime = 0;
        AttackTimer = 0;
        DelayTimer = 0;
        DamageStart = false;
        Targets = new List<LogicActor>();
        _selector = SkillSystem.GetSelector(skillInfo.Config.DamageAreaType);
        Effectors = SkillSystem.GetSkillEffector(skillInfo);
        OnStart();
        TakeReleaseEffector();
    }

    void ISkillExcutor.Update(FixInt deltaTime)
    {
        OnUpdate(deltaTime);

        ExcuteEffects(deltaTime);
        TakeDamage(deltaTime);
    }

    void ISkillExcutor.Dispose()
    {
        TakeEndEffector();
        OnDispose();
        Active = false;
        LockTarget = default;
        Targets.Clear();
        _selector = null;
        SkillSystem.RecycleEffectorItems(ref Effectors);
    }

    public void StartDamage()
    {
        if (DamageStart) return;

        DamageStart = true;
        Coefficient = SkillInfo.Data.Magnification / 10000f;
        Duration = SkillInfo.Config.Duration / 1000f;
        Interval = SkillInfo.Config.Interval / 1000f;
        AttackTime = 0;
        AttackTimer = 0;
        DelayTimer = SkillInfo.Config.Delay / 1000f;
    }

    protected virtual void OnStart() { }
    protected virtual void OnUpdate(FixInt deltaTime) { }
    protected virtual void OnDispose() { }

    public virtual void SelectTargets()
    {
        Targets.Clear();
        _selector.Select(SkillInfo, this, Targets);
    }


    protected virtual void TakeDamage(FixInt deltaTime)
    {
        if (!DamageStart) return;

        if (DelayTimer > 0)
        {
            DelayTimer -= deltaTime;
            return;
        }

        if (AttackTime < Duration || AttackTimer == 0)
        {
            if (AttackTime == 0 || AttackTimer >= Interval)
            {
                SelectTargets();
                switch (SkillInfo.Config.DestoryType)
                {
                    case EDestoryType.Trigger:
                        if (Targets.Count > 0)
                        {
                            foreach (var entity in Targets)
                            {
                                TakeDamage(SkillInfo.Owner, entity, SkillInfo);
                            }
                            this.As<ISkillExcutor>().Dispose();
                        }
                        break;
                    case EDestoryType.Time:
                        foreach (var entity in Targets)
                        {
                            TakeDamage(SkillInfo.Owner, entity, SkillInfo);
                        }
                        break;
                }
                if (AttackTime != 0)
                {
                    AttackTimer -= Interval;
                }
            }

            AttackTime += deltaTime;
            AttackTimer += deltaTime;
        }
        else
        {
            this.As<ISkillExcutor>().Dispose();
        }
    }

    void ExcuteEffects(FixInt deltaTime)
    {
        if (Effectors != null)
        {
            for (int i = 0; i < Effectors.Length; i++)
            {
                Effectors[i].Effector?.Excute(this, SkillInfo.Owner, deltaTime, Effectors[i].EffectValue);
            }
        }
    }

    void TakeReleaseEffector()
    {
        if (Effectors == null) return;

        for (int i = 0; i < Effectors.Length; i++)
        {
            Effectors[i].Effector?.Start(this, SkillInfo.Owner, Effectors[i].EffectValue);

            if (Effectors[i].TriggerTiming != ETriggerTiming.OnRelease) continue;

            Effectors[i].Effector?.As<IReleaseSkillEffector>()?.TakeEffect(this, SkillInfo.Owner, Effectors[i].EffectValue);
        }
    }

    void TakeEndEffector()
    {
        if (Effectors == null) return;

        for (int i = 0; i < Effectors.Length; i++)
        {
            Effectors[i].Effector?.End(this, SkillInfo.Owner, Effectors[i].EffectValue);
        }
    }

    void TakeAfterEffector(LogicActor attacker, LogicActor target, ref DamageInfo damageInfo)
    {
        if (Effectors == null) return;

        for (int i = 1; i < Effectors.Length; i++)
        {
            if (Effectors[i].TriggerTiming != ETriggerTiming.AfterCalDamage) continue;

            Effectors[i].Effector?.As<ITakeDamageAfterEffector>()?.TakeEffect(this, attacker, target, ref damageInfo, Effectors[i].EffectValue);
        }
    }

    void TakeBeforeEffector(LogicActor attacker, LogicActor target, ref DamageInfo damageInfo)
    {
        if (Effectors == null) return;

        for (int i = 1; i < Effectors.Length; i++)
        {
            if (Effectors[i].TriggerTiming != ETriggerTiming.BeforeCalDamage) continue;

            Effectors[i].Effector?.As<ITakeDamageBeforeEffector>()?.TakeEffect(this, attacker, target, ref damageInfo, Effectors[i].EffectValue);
        }
    }

    void TakeDamage(LogicActor attacker, LogicActor target, SkillInfo skillInfo)
    {
        DamageInfo damageInfo = default;

        // 有先后顺序 效果器最先执行 目标的事件随后 最后是攻击方的事件
        TakeBeforeEffector(attacker, target, ref damageInfo);
        // target.Entity.EventBeforeCalDamage?.Invoke(ref damageInfo, attacker.Entity, target.Entity, skillInfo.Data.ID);
        // attacker.Entity.EventBeforeCalDamage?.Invoke(ref damageInfo, attacker.Entity, target.Entity, skillInfo.Data.ID);

        // 如果效果器提供了一个合法的伤害信息，则不需要走伤害计算公式
        if (!damageInfo.IsValid)
        {
            damageInfo = DamageFormula.CalDamage(null, attacker, target, Coefficient);
        }

        // 有先后顺序 效果器最先执行 目标的事件随后 最后是攻击方的事件
        TakeAfterEffector(attacker, target, ref damageInfo);
        // target.Entity.EventAfterCalDamage?.Invoke(ref damageInfo, attacker.Entity, target.Entity, skillInfo.Data.ID);
        // attacker.Entity.EventAfterCalDamage?.Invoke(ref damageInfo, attacker.Entity, target.Entity, skillInfo.Data.ID);
        // OnTakeDamage(target);

        // 如果伤害信息不合法 则不处理后续
        if (damageInfo.IsValid)
        {
            if (attacker.Camp == target.Camp)
            {
                // 同一阵营不扣血
            }
            else
            {
                target.AttributeSet.AddHP(attacker, -damageInfo.FinalDamageValue);

                DamageMarkFactory.Show(target.Rendering.HeadTrans.position, 1, ref damageInfo);
            }
        }
    }

}
