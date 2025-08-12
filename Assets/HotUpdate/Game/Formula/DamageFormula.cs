using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public static class DamageFormula
{
    public static DamageInfo CalDamage(Randomizer randomizer, LogicActor attacker, LogicActor target, FixInt skillMul)
    {
        DamageInfo result = new DamageInfo();
        result.IsValid = true;
        result.OtherValue = 0;
        result.IsCriticalStrike = false;
        result.IsSkill = false;
        result.DamageValue = (skillMul * attacker.AttributeSet.GetValue(EAttributeKey.AttackPower)).Value;
        result.FinalDamageValue = result.DamageValue;

        return result;
    }
}

public class Randomizer
{

}
