using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorAttribute : ReflectionAttribute
{
    public SelectorAttribute(EDamageAreaType type) : base(typeof(ISelector), type, 100)
    {
    }
}
