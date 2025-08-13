using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Pool;
using UnityEngine;
using Zenject;

public class SkillOwner
{
    [Inject] public ISkillSystem SkillSystem;
    public LogicActor LogicActor;

    public List<SkillInfo> SkillInfos;
    Dictionary<int, FixInt> _skillTimerDic;
    List<int> _skillTimerIDList;
    List<SkillEventInfo> _skillEvents;
    static ObjectPool<SkillEventInfo> _eventPool = new ObjectPool<SkillEventInfo>(EPoolType.Scalable, 50);

    public void Initialize(LogicActor actor)
    {
        LogicActor = actor;
        SkillInfos = new List<SkillInfo>();
        _skillTimerDic = new Dictionary<int, FixInt>();
        _skillTimerIDList = new List<int>();
        _skillEvents = new List<SkillEventInfo>();
    }

    public void LogicUpdate(FixInt deltaTime)
    {
        ExcuteEvent(deltaTime);
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

            _skillTimerDic[_skillTimerIDList[i]] -= deltaTime * 1000;
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

    public void ReleaseSkill(int skillID, FixIntVector3 dirOrPos, LogicActor lockTarget)
    {
        if (GetSkillTimer(skillID) > 0) return;

        SkillInfo skillInfo = GetSkillInfo(skillID);

        if (skillInfo.Config.ReleaseType == ESkillReleaseType.VectorSkill || skillInfo.Config.ReleaseType == ESkillReleaseType.SectorVectorSkill)
        {
            LogicActor.SetDirection(dirOrPos);
        }

        if (!string.IsNullOrEmpty(skillInfo.Config.AnimationName))
        {
            LogicActor.Rendering.PlayAnimation(skillInfo.Config.AnimationName);
            LogicActor.AIAgent.CanMove.Value++;
            LogicActor.AIAgent.CanReleseSkill.Value++;
            LogicActor.Velocity = FixIntVector3.zero;
            for (int i = 0; i < skillInfo.Config.Times.Length; i++)
            {
                SkillEventInfo skillEventInfo = _eventPool.SpawnByType();
                skillEventInfo.Time = skillInfo.Config.Times[i];
                skillEventInfo.Action = () =>
                {
                    SkillSystem.ExcuteSkill(skillInfo, dirOrPos, lockTarget);
                    LogicActor.Rendering.PlayAnimation("idle");
                };
                _skillEvents.Add(skillEventInfo);
            }
            SkillEventInfo overEvent = _eventPool.SpawnByType();
            overEvent.Time = skillInfo.Config.ActionDuration;
            overEvent.Action = () =>
            {
                LogicActor.AIAgent.CanMove.Value--;
                LogicActor.AIAgent.CanReleseSkill.Value--;
            };
            _skillEvents.Add(overEvent);
        }
        else
        {
            SkillSystem.ExcuteSkill(skillInfo, dirOrPos, lockTarget);
        }

        if (!string.IsNullOrEmpty(skillInfo.Config.ActionPrefabPath))
        {
            LogicActor.Rendering.PlayActionEffect(skillInfo.Config.ActionPrefabPath, LogicActor.Rendering.GetPartTrans(skillInfo.Config.ActionPrefabPos));
        }
        SetSkillTimer(skillID, skillInfo.Config.CD);
    }

    void ExcuteEvent(FixInt deltaTime)
    {
        for (int i = 0; i < _skillEvents.Count; i++)
        {
            SkillEventInfo eventInfo = _skillEvents[i];
            if (!eventInfo.Invoke && eventInfo.Timer >= eventInfo.Time)
            {
                eventInfo.Invoke = true;
                eventInfo.Action?.Invoke();
            }
            else
            {
                eventInfo.Timer += deltaTime * 1000;
            }
        }

        for (int i = _skillEvents.Count - 1; i >= 0; i--)
        {
            SkillEventInfo eventInfo = _skillEvents[i];
            if (eventInfo.Invoke)
            {
                _eventPool.Release(eventInfo);
                _skillEvents.Remove(eventInfo);
            }
        }
    }

    public class SkillEventInfo
    {
        public FixInt Time;
        public FixInt Timer;
        public Action Action;
        public bool Invoke;

        public void Reset()
        {
            Invoke = false;
            Time = 0;
            Timer = 0;
            Action = null;
        }
    }
}
