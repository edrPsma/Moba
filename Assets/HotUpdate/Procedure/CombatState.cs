using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;

public class CombatState : BaseState
{
    public CombatState(bool hasExitTime = false) : base(hasExitTime) { }

    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入战斗流程");

        GameEntry.UI.HideGroup(UIGroup.Rear);
        GameEntry.UI.Pop<LoadingGameForm>();
    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出战斗流程");

        GameEntry.UI.ShowGroup(UIGroup.Rear);
    }
}
