using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public interface IReleaseSkillEffector
{
    void TakeEffect(SkillExcutor excutor, LogicActor skillOwner, List<int> param);
}

public interface ITakeDamageBeforeEffector
{
    void TakeEffect(SkillExcutor excutor, LogicActor skillOwner, LogicActor target, ref DamageInfo damageInfo, List<int> param);
}

public interface ITakeDamageAfterEffector
{
    void TakeEffect(SkillExcutor excutor, LogicActor skillOwner, LogicActor target, ref DamageInfo damageInfo, List<int> param);
}

public interface ISkillEffector : IReleaseSkillEffector, ITakeDamageBeforeEffector, ITakeDamageAfterEffector
{
    void Start(SkillExcutor excutor, LogicActor skillOwner, List<int> param);
    void Excute(SkillExcutor excutor, LogicActor skillOwner, FixInt deltaTime, List<int> param);
    void End(SkillExcutor excutor, LogicActor skillOwner, List<int> param);
}
