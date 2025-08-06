using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public interface IOperateSystem : ILogicController
{

}

[Controller]
public class OperateSystem : AbstarctController, IOperateSystem
{
    public void LogicUpdate(FixInt deltaTime)
    {

    }
}