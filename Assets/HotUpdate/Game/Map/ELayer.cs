using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum ELayer
{
    None = 0,
    Layer1 = 1,
    Layer2 = 2,
    All = Layer1 | Layer2,
}
