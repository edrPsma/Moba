using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;

public class LobbyState : BaseState
{
    public LobbyState(bool hasExitTime = false) : base(hasExitTime) { }

    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入大厅流程");

        GameEntry.UI.PushAsync<LobbyForm>();
    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出大厅流程");
    }
}
