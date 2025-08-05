using System;
using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;

public class LoadConfigState : BaseState
{
    public LoadConfigState(bool hasExitTime = false) : base(hasExitTime) { }

    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入加载配置表流程");

        DataTable.Initialize();
        DataTable.Progress.Register(OnProgressChange);
    }

    private void OnProgressChange(float value)
    {
        if (value >= 1)
        {
            DTHero[] heroes = DataTable.GetArray<DTHero>();
            GameEntry.Procedure.TransitionImmediately(EGameState.Lobby);
        }
    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出加载配置表流程");

        DataTable.Progress.UnRegister(OnProgressChange);
    }
}
