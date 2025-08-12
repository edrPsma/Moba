using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public class BaseEffector : ISkillEffector
{
    void ISkillEffector.Start(SkillExcutor excutor, LogicActor skillOwner, List<int> param)
    {
        OnStart(excutor, skillOwner, param);
    }

    void ISkillEffector.Excute(SkillExcutor excutor, LogicActor skillOwner, FixInt deltaTime, List<int> param)
    {
        OnExcute(excutor, skillOwner, deltaTime, param);
    }

    void IReleaseSkillEffector.TakeEffect(SkillExcutor excutor, LogicActor skillOwner, List<int> param)
    {
        OnTakeReleaseSkillEffect(excutor, skillOwner, param);
    }

    void ITakeDamageBeforeEffector.TakeEffect(SkillExcutor excutor, LogicActor skillOwner, LogicActor target, ref DamageInfo damageInfo, List<int> param)
    {
        OnTakeDamageBeforeEffect(excutor, skillOwner, target, ref damageInfo, param);
    }

    void ITakeDamageAfterEffector.TakeEffect(SkillExcutor excutor, LogicActor skillOwner, LogicActor target, ref DamageInfo damageInfo, List<int> param)
    {
        OnTakeDamageAfterEffect(excutor, skillOwner, target, ref damageInfo, param);
    }

    void ISkillEffector.End(SkillExcutor excutor, LogicActor skillOwner, List<int> param)
    {
        OnEnd(excutor, skillOwner, param);
    }

    protected virtual void OnStart(SkillExcutor excutor, LogicActor skillOwner, List<int> param)
    {

    }

    // Order:1
    protected virtual void OnTakeReleaseSkillEffect(SkillExcutor excutor, LogicActor skillOwner, List<int> param)
    {

    }

    // Order:2
    protected virtual void OnTakeDamageBeforeEffect(SkillExcutor excutor, LogicActor skillOwner, LogicActor target, ref DamageInfo damageInfo, List<int> param)
    {

    }

    // Order:3
    protected virtual void OnTakeDamageAfterEffect(SkillExcutor excutor, LogicActor skillOwner, LogicActor target, ref DamageInfo damageInfo, List<int> param)
    {

    }

    protected virtual void OnExcute(SkillExcutor excutor, LogicActor skillOwner, FixInt deltaTime, List<int> param)
    {

    }

    protected virtual void OnEnd(SkillExcutor excutor, LogicActor skillOwner, List<int> param)
    {

    }
}
