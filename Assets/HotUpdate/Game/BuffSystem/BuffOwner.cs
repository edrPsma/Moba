using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FixedPointNumber;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

public class BuffOwner : IBuffOwner
{
    [Inject] public ISkillSystem SkillSystem;
    public LogicActor Owner { get; private set; }

    [ShowInInspector] Dictionary<int, Buff> _buffDic;
    [ShowInInspector] Dictionary<int, int> _buffGroupDic;// buff组字典

    public void Initialize(LogicActor actor)
    {
        Owner = actor;
        _buffDic = new Dictionary<int, Buff>();
        _buffGroupDic = new Dictionary<int, int>();
    }

    public void LogicUpdate(FixInt deltaTime)
    {
        Buff[] buffs = _buffDic.Values.ToArray();

        for (int i = 0; i < buffs.Length; i++)
        {
            buffs[i].Update(deltaTime);
        }
    }

    public void AddBuff(int buffId, LogicActor caster, int skillID)
    {
        Buff buff = SkillSystem.Create(buffId, caster, Owner);

        // 如果重复添加 则用新的覆盖旧的
        if (_buffDic.ContainsKey(buffId))
        {
            _buffDic[buffId].Over();
        }

        _buffDic.Add(buffId, buff);
        buff.Start();
    }

    public void RemoveBuff(Buff buff)
    {
        if (_buffDic.ContainsKey(buff.Data.ID))
        {
            _buffDic.Remove(buff.Data.ID);
            SkillSystem.ReleaseBuff(buff);
        }
    }

    void RemoveAllBuff()
    {
        Buff[] buffs = _buffDic.Values.ToArray();
        foreach (var item in buffs)
        {
            RemoveBuff(item);
        }
        _buffDic.Clear();
    }

    public void Disperse(int buffID)
    {
        _buffDic.GetValue(buffID)?.Disperse();
    }

    public Buff GetBuff(int buffID)
    {
        if (_buffDic.ContainsKey(buffID))
        {
            return _buffDic[buffID];
        }

        return null;
    }

    public void Dispose()
    {
        RemoveAllBuff();
    }

    public void RemoveBuff(int buffID)
    {
        if (_buffDic.ContainsKey(buffID))
        {
            Buff buff = _buffDic[buffID];
            _buffDic.Remove(buffID);
            SkillSystem.ReleaseBuff(buff);
        }
    }

    public int[] GetAllBuffId()
    {
        return _buffDic.Keys.ToArray();
    }

    Buff[] IBuffOwner.GetAllBuff()
    {
        return _buffDic.Values.ToArray();
    }
}
