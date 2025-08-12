using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillRuleAttribute : ReflectionAttribute
{
    public SkillRuleAttribute(ESkillRuleType type) : base(typeof(ISkillExcutor), type, 100)
    {
    }
}
