using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EDamageAreaType
{
    /// <summary>
    /// 单体
    /// </summary>
    Monomer = 0,

    /// <summary>
    /// 圆形
    /// </summary>
    Round = 1,

    /// <summary>
    /// 矩形
    /// </summary>
    Rectangle = 2,

    /// <summary>
    /// 扇形
    /// </summary>
    Sector = 3,

    /// <summary>
    /// 偏移矩形
    /// </summary>
    OffsetRectangle = 4,
}
