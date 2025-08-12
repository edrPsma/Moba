using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public class CombatUtility
{
    /// <summary>
    /// long 钳制
    /// </summary>
    /// <param name="value">当前值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <returns></returns>
    public static long Clamp(long value, long min, long max)
    {
        if (value < min)
        {
            value = min;
        }
        if (value > max)
        {
            value = max;
        }

        return value;
    }

    public static void SelectActor(FixIntVector3 pos, FixInt area, ELayer layer, List<LogicActor> result)
    {
        IPhysicsSystem physicsSystem = MVCContainer.Get<IPhysicsSystem>();
        if (physicsSystem == null) return;

        int num = physicsSystem.OverlapSphere
        (
            pos,
            area,
            layer,
            ISelector.CacheArray
        );

        for (int i = 0; i < num; i++)
        {
            result.Add(ISelector.CacheArray[i]);
        }
    }

    public static ELayer GetSkillLayer(SkillConfig skillConfig, ECamp camp)
    {
        if (skillConfig.TargetType == ETargetType.Friendly)
        {
            if (camp == ECamp.Red)
            {
                return ELayer.Layer1;
            }
            else if (camp == ECamp.Blue)
            {
                return ELayer.Layer2;
            }
            else
            {
                return ELayer.Layer3;
            }
        }
        else
        {
            if (camp == ECamp.Red)
            {
                return ELayer.Layer2 | ELayer.Layer3;
            }
            else if (camp == ECamp.Blue)
            {
                return ELayer.Layer1 | ELayer.Layer3;
            }
            else
            {
                return ELayer.All;
            }
        }
    }

    public static LogicActor SelectClosestActor(FixIntVector3 pos, List<LogicActor> actors)
    {
        if (actors.Count <= 0) return null;

        LogicActor result = null;
        FixInt dist = FixInt.MaxValue;
        foreach (var item in actors)
        {
            FixInt curDist = (item.Position - pos).sqrMagnitude;
            if (curDist < dist)
            {
                result = item;
                dist = curDist;
            }
        }

        return result;
    }

}