using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FixedPointNumber;
using Pool;
using UnityEngine;
using Zenject;

public interface ISkillSystem : ILogicController
{
    void ExcuteSkill(SkillInfo skillInfo, FixIntVector3 dirOrPos, LogicActor lockTarget);

    SkillInfo SpawnSkillInfo(LogicActor actor, int skillId);

    ISelector GetSelector(EDamageAreaType type);

    EffectorItem[] GetSkillEffector(SkillInfo skillInfo);

    void RecycleEffectorItems(ref EffectorItem[] effectors);

    Buff Create(int buffID, LogicActor caster, LogicActor owner);

    void ReleaseBuff(Buff buff);

    IBuffExcutor GetBuffExcutor(int type);

    IBuffExcutor GetBuffExcutor(EBuffExcutorType type);
}

[Controller]
public class SkillSystem : AbstarctController, ISkillSystem
{
    [Inject] public IAssetSystem AssetSystem;
    ObjectPool<EffectorItem> _effectorItemPool;
    ObjectPool<Buff> _buffPool;

    List<ISkillExcutor> _excuteList;// 当前正在执行的技能
    List<ISkillExcutor> _removeList;// 待移除的技能

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _excuteList = new List<ISkillExcutor>();
        _removeList = new List<ISkillExcutor>();
        _effectorItemPool = new ObjectPool<EffectorItem>(EPoolType.Scalable, 50);
        _effectorItemPool.OnReleaseEvent += item => item.OnReset();
        _buffPool = new ObjectPool<Buff>(EPoolType.Scalable, 50);
        _buffPool.OnReleaseEvent += item => item.OnReset();
    }

    public void LogicUpdate(FixInt deltaTime)
    {
        for (int i = 0; i < _excuteList.Count; i++)
        {
            if (!_excuteList[i].Active)
            {
                _removeList.Add(_excuteList[i]);
            }
            else
            {
                _excuteList[i].Update(deltaTime);
            }
        }

        for (int i = 0; i < _removeList.Count; i++)
        {
            _excuteList.Remove(_removeList[i]);
        }

        _removeList.Clear();
    }

    public void ExcuteSkill(SkillInfo skillInfo, FixIntVector3 dirOrPos, LogicActor lockTarget)
    {
        // TODO 消耗MP
        // owner.AttributeSet.AddMP(skillInfo.Cost);

        var skillExcutor = SpawnSkill(skillInfo);
        if (skillExcutor == null) return;


        SetSkillDirOrPos(skillInfo, skillExcutor, dirOrPos, lockTarget);
        skillExcutor.LockTarget = lockTarget;

        skillExcutor.Start(skillInfo);
        _excuteList.Add(skillExcutor);
    }

    void SetSkillDirOrPos(SkillInfo skillInfo, ISkillExcutor excutor, FixIntVector3 dirOrPos, LogicActor lockTarget)
    {
        if (skillInfo.Config.ReleaseType == ESkillReleaseType.TargetUnit && skillInfo.Config.BulletPlace == EBulletPlace.Target)
        {
            excutor.Position = lockTarget.Position;
            excutor.Direction = FixIntVector3.forward;
        }
        else if (skillInfo.Config.ReleaseType == ESkillReleaseType.VectorSkill)
        {
            excutor.Position = skillInfo.Owner.Position;
            excutor.Direction = dirOrPos;
        }
        else if (skillInfo.Config.ReleaseType == ESkillReleaseType.TargetPoint)
        {
            excutor.Position = dirOrPos;
            excutor.Direction = FixIntVector3.forward;
        }
        else
        {
            excutor.Position = skillInfo.Owner.Position;
            excutor.Direction = FixIntVector3.forward;
        }
    }

    ISkillExcutor SpawnSkill(SkillInfo skillInfo)
    {
        if (!CheckIsValid(skillInfo)) return null;

        ISkillExcutor skillExcutor = GameEntry.Reflection.Get<ISkillExcutor>(skillInfo.Config.RuleType).New();
        if (skillExcutor == null)
        {
            Debug.LogError($"Combat 技能规则未定义 {skillInfo.Config.RuleType}");
        }

        return skillExcutor;
    }

    public ISelector GetSelector(EDamageAreaType type)
    {
        return GameEntry.Reflection.Get<ISelector>(type);
    }

    public SkillInfo SpawnSkillInfo(LogicActor actor, int skillId)
    {
        var data = DataTable.GetItem<DTSkill>(skillId);
        if (data == null) return null;

        SkillInfo skillInfo = new SkillInfo();
        skillInfo.Data = data;
        skillInfo.Config = AssetSystem.GetSkillConfig(skillId);
        skillInfo.Owner = actor;
        skillInfo.Layer = CombatUtility.GetSkillLayer(skillInfo.Config, actor.Camp);

        return skillInfo;
    }

    public EffectorItem[] GetSkillEffector(SkillInfo skillInfo)
    {
        if (skillInfo.Data.Effects != null)
        {
            int len = skillInfo.Data.Effects.Length;
            EffectorItem[] result = new EffectorItem[len];
            for (int i = 0; i < len; i++)
            {
                result[i] = GetSkillEffector(skillInfo.Data.Effects[i]);
            }

            return result;
        }
        else
        {
            return null;
        }
    }

    public EffectorItem GetSkillEffector(int id)
    {
        DTSkill_effect table = DataTable.GetItem<DTSkill_effect>(id);

        // 没有符合条件的效果器
        if (table == null)
        {
            Debug.LogError($"没有该效果器,ID: {id}");
            return null;
        }
        else
        {
            EffectorItem impactor = _effectorItemPool.SpawnByType();
            impactor.EffectId = table.ID;
            impactor.Type = table.Type;
            impactor.Effector = GameEntry.Reflection.Get<ISkillEffector>((EEffectType)table.Type);
            impactor.TriggerTiming = (ETriggerTiming)table.TriggerTimeing;
            impactor.EffectValue.AddRange(table.EffectValue);

            return impactor;
        }
    }

    public void RecycleEffectorItems(ref EffectorItem[] effectors)
    {
        if (effectors != null)
        {
            foreach (var item in effectors)
            {
                _effectorItemPool.Release(item);
            }

            effectors = null;
        }
    }

    bool CheckIsValid(SkillInfo skillInfo)
    {
        if (skillInfo == null)
        {
            Debug.LogError("Combat 技能信息为空");
            return false;
        }

        if (skillInfo.Data == null)
        {
            Debug.LogError("Combat 技能表配置为空");
            return false;
        }

        if (skillInfo.Config == null)
        {
            Debug.LogError("Combat 技能配置文件为空");
            return false;
        }

        return true;
    }

    public Buff Create(int buffID, LogicActor caster, LogicActor owner)
    {
        Buff buff = _buffPool.SpawnByType();
        buff.Init(DataTable.GetItem<DTSkill_buff>(buffID), caster, owner);

        return buff;
    }

    public void ReleaseBuff(Buff buff)
    {
        _buffPool.Release(buff);
    }

    public IBuffExcutor GetBuffExcutor(int type)
    {
        return GetBuffExcutor((EBuffExcutorType)type);
    }

    public IBuffExcutor GetBuffExcutor(EBuffExcutorType type)
    {
        return GameEntry.Reflection.Get<IBuffExcutor>(type);
    }
}
