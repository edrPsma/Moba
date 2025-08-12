using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Selector(EDamageAreaType.Monomer)]
public class SingleTargetSelector : BaseSelector
{
    public override void Select(SkillInfo info, ISkillExcutor skillExcutor, List<LogicActor> infos)
    {
        if (skillExcutor.LockTarget != null)
        {
            infos.Add(skillExcutor.LockTarget);
        }
    }

    public override void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor)
    {

    }
}