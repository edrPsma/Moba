using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using Zenject;

public class SkillOwner
{
    [Inject] public ISkillSystem SkillSystem;
    public LogicActor LogicActor;

    public List<SkillInfo> SkillInfos;
    Dictionary<int, FixInt> _skillTimerDic;
    List<int> _skillTimerIDList;

    public void Initialize(LogicActor actor)
    {
        LogicActor = actor;
        SkillInfos = new List<SkillInfo>();
        _skillTimerDic = new Dictionary<int, FixInt>();
        _skillTimerIDList = new List<int>();
    }

    public void LogicUpdate(FixInt deltaTime)
    {
        SkillCoolDown(deltaTime);
    }

    public SkillInfo GetSkillInfo(int skillID)
    {
        for (int i = 0; i < SkillInfos.Count; i++)
        {
            if (SkillInfos[i].Data.ID == skillID)
            {
                return SkillInfos[i];
            }
        }

        return null;
    }

    public void BindSkill(int[] skills)
    {
        for (int i = 0; i < skills.Length; i++)
        {
            SkillInfo skillInfo = SkillSystem.SpawnSkillInfo(LogicActor, skills[i]);
            if (skillInfo != null)
            {
                SkillInfos.Add(skillInfo);
            }
        }
    }

    void SkillCoolDown(FixInt deltaTime)
    {
        for (int i = 0; i < _skillTimerIDList.Count; i++)
        {
            if (_skillTimerDic[_skillTimerIDList[i]] <= 0) continue;

            _skillTimerDic[_skillTimerIDList[i]] -= deltaTime;
        }
    }

    public void SetSkillTimer(int skillID, FixInt time)
    {
        if (!_skillTimerDic.ContainsKey(skillID))
        {
            _skillTimerDic.Add(skillID, time);
            _skillTimerIDList.Add(skillID);
        }
        else
        {
            _skillTimerDic[skillID] = time;
        }
    }

    public FixInt GetSkillTimer(int skillID)
    {
        if (_skillTimerDic.ContainsKey(skillID))
        {
            return _skillTimerDic[skillID];
        }
        else
        {
            return 0;
        }
    }
}
