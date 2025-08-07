using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UI;
using UnityEngine;
using Zenject;

public class PlayForm : UIForm
{
    public override UIGroup DefultGroup => UIGroup.FrontMost;
    public override string Location => "Assets/GameAssets/UIPrefab/PlayWnd.prefab";

    [Inject] public ICombatSystem CombatSystem;
    [Inject] public IOperateSystem OperateSystem;

    FloatingJoystick _joystick;

    protected override void OnStart()
    {
        base.OnStart();
        _joystick = this.Get<FloatingJoystick>("Joystick");
        CombatSystem.OnLogicUpdate += OnLogicUpdate;
    }

    private void OnLogicUpdate(FixInt deltaTime)
    {
        Vector3 dir = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;
        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Vector3 rotated = rotation * dir;
        OperateSystem.SendMoveOperate(new FixIntVector3(rotated));
    }
}
