using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SkillRule(ESkillRuleType.None)]
public class CommonRule : SkillExcutor
{
    public override ISkillExcutor New() => new CommonRule();

    protected override void OnStart()
    {
        StartDamage();
    }
}
