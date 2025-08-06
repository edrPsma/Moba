using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public interface ICommandSystem : ILogicController
{

}

[Controller]
public class CommandSystem : AbstarctController, ICommandSystem
{
    public void LogicUpdate(FixInt deltaTime)
    {

    }
}
