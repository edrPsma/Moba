using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

[Effector(EEffectType.Flash)]
public class FlashEffector : BaseEffector
{
    protected override void OnTakeReleaseSkillEffect(SkillExcutor excutor, LogicActor skillOwner, List<int> param)
    {
        base.OnTakeReleaseSkillEffect(excutor, skillOwner, param);

        FixIntVector3 pos = excutor.Position + excutor.Direction.normalized * param[0];
        excutor.SkillInfo.Owner.SetPosition(pos);
        excutor.SkillInfo.Owner.SetDirection(excutor.Direction);
        excutor.SkillInfo.Owner.Rendering.PlayActionEffect("Flash_magic_blue_pink", excutor.SkillInfo.Owner.Rendering.BodyTrans);
    }
}
