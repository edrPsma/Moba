using System.Collections;
using System.Collections.Generic;
using Observable;
using UnityEngine;

public class AIAgent
{
    public LogicActor LogicActor;
    public IntVariable CanMove;
    public IntVariable CanReleseSkill;

    public void Initialize(LogicActor actor)
    {
        LogicActor = actor;
        CanMove = new IntVariable(0, 0, int.MaxValue);
        CanReleseSkill = new IntVariable(0, 0, int.MaxValue);
    }
}
