using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ELayer
{
    None = 0,

    /// <summary>
    /// 对应红方
    /// </summary>
    Layer1 = 1,

    /// <summary>
    /// 对应蓝方
    /// </summary>
    Layer2 = 2,

    /// <summary>
    /// 对应中立
    /// </summary>
    Layer3 = 4,


    All = Layer1 | Layer2 | Layer3,
}
