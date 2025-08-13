using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

[Buff(EBuffExcutorType.Cure)]
public class CureBuff : BuffExcutor
{
    protected override void OnUpdate(Buff buff, int id, int[] parms)
    {
        base.OnUpdate(buff, id, parms);

        FixInt maxHp = buff.Owner.AttributeSet.HPAttribute.Max;
        FixInt ratio = parms[0] / 10000f;
        FixInt cureValue = maxHp * ratio;

        buff.Owner.AttributeSet.AddHP(buff.Owner, cureValue);
        DamageMarkFactory.ShowRecover(buff.Owner.Rendering.HeadTrans.position, 1, cureValue.RawInt);
    }
}
