using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public class Buff
{
    public ISkillSystem SkillSystem => MVCContainer.Get<ISkillSystem>();
    public LogicActor Owner { get; private set; }
    public LogicActor Caster { get; private set; }
    public DTSkill_buff Data { get; private set; }
    public FixIntVariable Duration { get; private set; }
    public FixInt Timer { get; private set; }
    public bool IsActive { get; private set; }
    // public Blackboard Blackboard { get; private set; }
    public IBuffExcutor _excutor;

    FixInt _interval;

    public Buff()
    {
        Duration = new FixIntVariable(0);
        // Blackboard = new Blackboard();
    }

    public void Init(DTSkill_buff data, LogicActor caster, LogicActor owner)
    {
        Data = data;
        Caster = caster;
        Owner = owner;
    }

    public void Start()
    {
        IsActive = true;
        Duration.Value = Data.Duration / 1000f;
        _interval = Data.Interval / 1000f;

        _excutor = SkillSystem.GetBuffExcutor(Data.Effect);
        if (_excutor != null)
        {
            _excutor.Start(this, Data.ID, Data.EffectValue);
        }
        else
        {
            Debug.LogError($"Combat Buff执行器为空,Type: {Data.Effect}");
            Owner.BuffOwner.RemoveBuff(this);
        }
    }

    public void Update(FixInt deltaTime)
    {
        if (!IsActive) return;

        if ((Data.Duration != 0 && Duration.Value >= 0) || Data.Duration == 0)
        {
            try
            {
                if (Timer <= 0)
                {
                    _excutor?.Update(this, Data.ID, Data.EffectValue);
                }
                Timer += deltaTime;
                if (Timer >= _interval)
                {
                    Timer -= _interval;
                }

                if (Data.Duration != 0)
                {
                    Duration.Value -= deltaTime;
                    if (Duration.Value <= 0)
                    {
                        Over();
                    }
                }
            }
            catch (Exception e) { Debug.LogError(e); }
        }
        else
        {
            Owner.BuffOwner.RemoveBuff(this);
        }
    }

    public void Over()
    {
        if (!IsActive) return;

        IsActive = false;
        _excutor.Over(this, Data.ID, Data.EffectValue);
        Owner.BuffOwner.RemoveBuff(this);
    }

    public void Disperse()
    {
        if (!IsActive) return;
        // if (Data.CanPurify == DTSkill_buff.ECanPurify.Bu_neng) return;

        IsActive = false;
        _excutor.Interrupt(this, Data.ID, Data.EffectValue);
        _excutor.Over(this, Data.ID, Data.EffectValue);
        Owner.BuffOwner.RemoveBuff(this);
    }

    public void OnReset()
    {
        Owner = default;
        Caster = default;
        Duration.UnRegisterAll();
        Duration.Value = 0;
        IsActive = false;
        // Blackboard.Reset();
        _excutor = null;
    }
}
