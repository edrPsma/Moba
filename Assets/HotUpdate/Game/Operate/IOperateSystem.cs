using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Protocol;
using UnityEngine;
using Zenject;

public interface IOperateSystem : ILogicController
{
    event Action<FixInt> OnLogicUpdate;

    void Input(Operate operate);

    void SendMoveOperate(FixIntVector3 dir);
}

[Controller]
public class OperateSystem : AbstarctController, IOperateSystem
{
    [Inject] public IPlayerModel PlayerModel;
    [Inject] public IActorManager ActorManager;
    [Inject] public ICombatSystem CombatSystem;

    List<Operate> _operates = new List<Operate>();
    public event Action<FixInt> OnLogicUpdate;

    public void LogicUpdate(FixInt deltaTime)
    {
        OnLogicUpdate?.Invoke(deltaTime);

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

    public void SendMoveOperate(FixIntVector3 dir)
    {
        if (!CombatSystem.InCombat) return;
        if (!CombatSystem.CanOperate.Value) return;

        U2GS_Operate operate = new U2GS_Operate();
        operate.Type = 1;
        Vector vector = new Vector();
        vector.X = dir.x.Value;
        vector.Z = dir.z.Value;
        operate.MoveOperate = new MoveOperate
        {
            Velocity = vector
        };
        GameEntry.Net.SendMsg(operate);

        // Operate operate = new Operate();
        // operate.Type = 1;
        // operate.UId = PlayerModel.UID;
        // Vector vector = new Vector();
        // vector.X = dir.x.Value;
        // vector.Z = dir.z.Value;
        // operate.MoveOperate = new MoveOperate
        // {
        //     Velocity = vector
        // };

        // CommandSystem.Input(operate);
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

        if (heroActor != null)
        {
            heroActor.Velocity = new FixIntVector3(x, 0, z) * heroActor.MoveSpeed;
        }
    }

    void ExcuteSkillOperate(Operate operate)
    {

    }
}