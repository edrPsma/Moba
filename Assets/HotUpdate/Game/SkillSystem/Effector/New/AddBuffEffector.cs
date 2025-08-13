using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Effector(EEffectType.AddBuff)]
public class AddBuffEffector : BaseEffector
{
    protected override void OnTakeDamageAfterEffect(SkillExcutor excutor, LogicActor skillOwner, LogicActor target, ref DamageInfo damageInfo, List<int> param)
    {
        base.OnTakeDamageAfterEffect(excutor, skillOwner, target, ref damageInfo, param);

        skillOwner.BuffOwner.AddBuff(param[0], target, excutor.SkillInfo.Data.ID);
    }

    protected override void OnTakeDamageBeforeEffect(SkillExcutor excutor, LogicActor skillOwner, LogicActor target, ref DamageInfo damageInfo, List<int> param)
    {
        base.OnTakeDamageBeforeEffect(excutor, skillOwner, target, ref damageInfo, param);

        skillOwner.BuffOwner.AddBuff(param[0], target, excutor.SkillInfo.Data.ID);
    }

    protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, LogicActor skillOwner, List<int> param)
    {
        base.OnTakeReleaseSkillEffect(excutor, skillOwner, param);

        skillOwner.BuffOwner.AddBuff(param[0], skillOwner, excutor.SkillInfo.Data.ID);
    }
}
