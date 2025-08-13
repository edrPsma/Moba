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

    void SendSkillOperate(int skillID, FixIntVector3 dirOrPos, int targetID);
}

[Controller]
public class OperateSystem : AbstarctController, IOperateSystem
{
    [Inject] public IPlayerModel PlayerModel;
    [Inject] public IActorManager ActorManager;
    [Inject] public ICombatSystem CombatSystem;
    [Inject] public ISkillSystem SkillSystem;

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
        HeroActor heroActor = ActorManager.GetSelfHero();
        if (heroActor.AIAgent.CanMove.Value != 0) return;

        if (!CombatSystem.InCombat) return;
        if (!CombatSystem.CanOperate.Value) return;

        if (PlayerModel.GameConfig.TestMode)
        {
            Operate operate = new Operate();
            operate.Type = 1;
            operate.UId = PlayerModel.UID;
            Vector vector = new Vector();
            vector.X = dir.x.Value;
            vector.Z = dir.z.Value;
            operate.MoveOperate = new MoveOperate
            {
                Velocity = vector
            };

            Input(operate);
        }
        else
        {
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
        }
    }

    public void SendSkillOperate(int skillID, FixIntVector3 dirOrPos, int targetID)
    {
        HeroActor heroActor = ActorManager.GetSelfHero();
        if (heroActor.AIAgent.CanReleseSkill.Value != 0) return;

        if (!CombatSystem.InCombat) return;
        if (!CombatSystem.CanOperate.Value) return;

        if (PlayerModel.GameConfig.TestMode)
        {
            Operate operate = new Operate();
            operate.Type = 2;
            operate.UId = PlayerModel.UID;
            Vector vector = new Vector();
            vector.X = dirOrPos.x.Value;
            vector.Y = dirOrPos.y.Value;
            vector.Z = dirOrPos.z.Value;
            operate.SkillOperate = new SkillOperate
            {
                SkillID = skillID,
                DirOrPos = vector,
                Target = targetID
            };

            Input(operate);
        }
        else
        {
            U2GS_Operate operate = new U2GS_Operate();
            operate.Type = 2;

            Vector vector = new Vector();
            vector.X = dirOrPos.x.Value;
            vector.Y = dirOrPos.y.Value;
            vector.Z = dirOrPos.z.Value;
            operate.SkillOperate = new SkillOperate
            {
                SkillID = skillID,
                DirOrPos = vector,
                Target = targetID
            };
            GameEntry.Net.SendMsg(operate);
        }
    }

    public void Input(Operate operate)
    {
        _operates.Add(operate);
    }

    void ExcuteMoveOperate(Operate operate)
    {
        HeroActor heroActor = ActorManager.GetHero(operate.UId);

        if (heroActor.AIAgent.CanMove.Value != 0) return;

        FixInt x = operate.MoveOperate.Velocity.X;
        FixInt z = operate.MoveOperate.Velocity.Z;

        if (heroActor != null)
        {
            heroActor.Velocity = new FixIntVector3(x, 0, z) * heroActor.MoveSpeed;
        }
    }

    void ExcuteSkillOperate(Operate operate)
    {
        HeroActor heroActor = ActorManager.GetHero(operate.UId);

        if (heroActor.AIAgent.CanReleseSkill.Value != 0) return;

        int skillID = operate.SkillOperate.SkillID;
        FixIntVector3 vector = new FixIntVector3(operate.SkillOperate.DirOrPos.X, operate.SkillOperate.DirOrPos.Y, operate.SkillOperate.DirOrPos.Z);
        int targetID = operate.SkillOperate.Target;

        heroActor.SkillOwner.ReleaseSkill(skillID, vector, ActorManager.GetActor(targetID));
    }
}