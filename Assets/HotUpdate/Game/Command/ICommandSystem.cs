using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Protocol;
using UnityEngine;
using Zenject;

public interface ICommandSystem : ILogicController
{
    void Input(Operate operate);
}

[Controller]
public class CommandSystem : AbstarctController, ICommandSystem
{
    [Inject] public IActorManager ActorManager;

    List<Operate> _operates = new List<Operate>();

    public void LogicUpdate(FixInt deltaTime)
    {
        for (int i = 0; i < _operates.Count; i++)
        {
            Operate operate = _operates[i];
            if (operate.Type == 1)
            {
                ExcuteMoveOperate(operate);
            }
            else
            {
                ExcuteSkillOperate(operate);
            }
        }
        _operates.Clear();
    }

    public void Input(Operate operate)
    {
        _operates.Add(operate);
    }

    void ExcuteMoveOperate(Operate operate)
    {
        HeroActor heroActor = ActorManager.GetHero(operate.UId);
        FixInt x = operate.MoveOperate.Velocity.X;
        FixInt z = operate.MoveOperate.Velocity.Z;
        heroActor.Velocity = new FixIntVector3(x, 0, z) * heroActor.MoveSpeed;
    }

    void ExcuteSkillOperate(Operate operate)
    {

    }
}
