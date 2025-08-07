using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Protocol;
using UnityEngine;
using Zenject;

public interface IOperateSystem : ILogicController
{
    void SendMoveOperate(FixIntVector3 dir);
}

[Controller]
public class OperateSystem : AbstarctController, IOperateSystem
{
    [Inject] public IPlayerModel PlayerModel;
    [Inject] public ICommandSystem CommandSystem;

    public void LogicUpdate(FixInt deltaTime)
    {
        // float x = Input.GetAxisRaw("Horizontal");
        // float z = Input.GetAxisRaw("Vertical");

        // SendMoveOperate(new FixIntVector3(new Vector3(x, 0, z).normalized));
    }

    public void SendMoveOperate(FixIntVector3 dir)
    {
        // U2GS_Operate operate = new U2GS_Operate();
        // operate.Type = 1;
        // Vector vector = new Vector();
        // vector.X = velocity.x.Value;
        // vector.Z = velocity.z.Value;
        // operate.MoveOperate = new MoveOperate
        // {
        //     Velocity = vector
        // };

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

        CommandSystem.Input(operate);
    }
}