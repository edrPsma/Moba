using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Extension
{
    /// <summary>
    /// 强制转换
    /// </summary>
    /// <typeparam name="T">目标类型</typeparam>
    /// <param name="target">目标</param>
    /// <returns></returns>
    public static T As<T>(this object target) where T : class
    {
        return target as T;
    }
}
