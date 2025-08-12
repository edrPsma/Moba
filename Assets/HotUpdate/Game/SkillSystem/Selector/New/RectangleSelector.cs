using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

[Selector(EDamageAreaType.Rectangle)]
public class RectangleSelector : BaseSelector
{
    public override void Select(SkillInfo info, ISkillExcutor skillExcutor, List<LogicActor> infos)
    {
        int num = PhysicsSystem.OverlapBox
        (
            skillExcutor.Position,
            new FixIntVector3(info.Config.DamageArea[0], 5, info.Config.DamageArea[1]),
            skillExcutor.Direction,
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