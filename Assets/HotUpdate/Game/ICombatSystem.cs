using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using Zenject;

public interface ICombatSystem : ILogicController
{

}

[Controller]
public class CombatSystem : AbstarctController, ICombatSystem
{
    [Inject] public IOperateSystem OperateSystem;
    [Inject] public IMoveSystem MoveSystem;
    [Inject] public ICommandSystem CommandSystem;

    public void LogicUpdate(FixInt deltaTime)
    {
        OperateSystem.LogicUpdate(deltaTime);// 向服务器发送操作
        CommandSystem.LogicUpdate(deltaTime);// 服务器下发操作
        MoveSystem.LogicUpdate(deltaTime);// 移动
    }
}