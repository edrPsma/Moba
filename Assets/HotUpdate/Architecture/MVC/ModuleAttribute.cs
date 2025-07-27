using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleAttribute : Attribute
{
    public Type[] Types;

    public ModuleAttribute(params Type[] types)
    {
        Types = types;
    }
}