using System.Collections;
using System.Collections.Generic;
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
}