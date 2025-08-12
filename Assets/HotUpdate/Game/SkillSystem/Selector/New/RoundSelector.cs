using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Selector(EDamageAreaType.Round)]
public class RoundSelector : BaseSelector
{
    public override void Select(SkillInfo info, ISkillExcutor skillExcutor, List<LogicActor> infos)
    {
        int num = PhysicsSystem.OverlapSphere
        (
            skillExcutor.Position,
            info.Config.DamageArea[0],
            info.Layer,
            ISelector.CacheArray
        );

        for (int i = 0; i < num; i++)
        {
            infos.Add(ISelector.CacheArray[i]);
        }
    }

    public override void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor)
    {

    }
}
