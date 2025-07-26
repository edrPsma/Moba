using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;

public class SystemInitState : BaseState
{
    public SystemInitState(bool hasExitTime = false) : base(hasExitTime) { }

    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入系统初始化流程");
    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出系统初始化流程");
    }
}
