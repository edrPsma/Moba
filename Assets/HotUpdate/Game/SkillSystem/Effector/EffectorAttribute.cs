using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectorAttribute : ReflectionAttribute
{
    public EffectorAttribute(EEffectType effectType) : base(typeof(ISkillEffector), effectType, 100)
    {
    }
}
