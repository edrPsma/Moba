using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using OBB;
using UnityEngine;

[Selector(EDamageAreaType.Sector)]
public class SectorSelector : BaseSelector
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
            OBBCapsuleCollider collider = ISelector.CacheArray[i].Collider as OBBCapsuleCollider;
            FixInt scaleR = FixIntMath.Max(collider.Scale.x, collider.Scale.z);
            FixInt r = scaleR * collider.Radius;

            if (!InArea(skillExcutor.Position, ISelector.CacheArray[i].Position, r, info.Config.DamageArea[1], skillExcutor.Direction)) continue;

            infos.Add(ISelector.CacheArray[i]);
        }
    }

    public override void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor)
    {

    }

    bool InArea(FixIntVector3 pos, FixIntVector3 targetPos, FixInt outRadius, FixInt angle, FixIntVector3 direction)
    {
        FixIntVector3 dir = targetPos - pos;
        FixInt dist = dir.magnitude;
        FixInt ratio = outRadius / dist;
        if (ratio > 1f) ratio = 1f; // 避免超范围
        FixInt angleOffset = 90f - FixIntMath.Acos(ratio) * FixIntMath.Rad2Deg;

        FixInt curAngle = FixIntVector3.Angle(direction, dir);

        return angle * 0.5f + angleOffset >= curAngle;
    }
}
