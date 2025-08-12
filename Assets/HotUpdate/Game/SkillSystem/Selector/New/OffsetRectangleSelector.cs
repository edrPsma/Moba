using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using Drawing;
using Unity.Mathematics;

[Selector(EDamageAreaType.OffsetRectangle)]
public class OffsetRectangleSelector : BaseSelector
{
    public override void Select(SkillInfo info, ISkillExcutor skillExcutor, List<LogicActor> infos)
    {
        int num = PhysicsSystem.OverlapBox
        (
            skillExcutor.Position + skillExcutor.Direction * info.Config.DamageArea[0] * 0.5f,
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
        // Draw.WireBox(new float3(skillExcutor.Position.ToVector3()),
        //         transform.rotation,
        //         new float3(info.Config.DamageArea[0], 5, info.Config.DamageArea[1]),
        //         Color.red
        //      );
    }
}