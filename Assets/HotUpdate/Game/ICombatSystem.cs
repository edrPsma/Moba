using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Observable;
using Protocol;
using UnityEngine;
using Zenject;

public interface ICombatSystem : ILogicController
{
    int FrameID { get; }

    int TargetFrameID { get; }

    bool InCombat { get; }

    BoolVariable CanOperate { get; }

    void StartCombat();

    void StopCombat();
}

[Controller]
public class CombatSystem : AbstarctController, ICombatSystem
{
    [Inject] public IOperateSystem OperateSystem;
    [Inject] public IPhysicsSystem MoveSystem;
    [Inject] public IActorManager ActorManager;
    [Inject] public IPlayerModel PlayerModel;

    public int FrameID { get; private set; }
    public bool InCombat { get; private set; }
    public BoolVariable CanOperate { get; private set; }

    public int TargetFrameID { get; private set; } = -1;

    Queue<GS2U_Operate> _missOperates;// 因断线而丢失的指令列表
    Queue<GS2U_Operate> _operates;// 服务器下发的指令列表

    int _testModelTaslID;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _missOperates = new Queue<GS2U_Operate>();
        _operates = new Queue<GS2U_Operate>();
        CanOperate = new BoolVariable();

        GameEntry.Net.Register<GS2U_Operate>(OnOperate);
        GameEntry.Net.Register<GS2U_MissOperate>(OnMissOperate);
    }


    public void LogicUpdate(FixInt deltaTime)
    {
        if (!InCombat) return;

        ProcressOperate();

        OperateSystem.LogicUpdate(deltaTime);// 操作处理
        MoveSystem.LogicUpdate(deltaTime);// 移动
        ActorManager.LogicUpdate(deltaTime);
    }

    public void StartCombat()
    {
        TargetFrameID = -1;
        FrameID = 0;
        _missOperates.Clear();
        _operates.Clear();
        InCombat = true;
        CanOperate.Value = false;

        if (PlayerModel.GameConfig.TestMode)
        {
            CanOperate.Value = true;
            _testModelTaslID = GameEntry.Task.AddTask(_ =>
            {
                LogicUpdate(new FixInt(0.0667));
            })
            .Delay(TimeSpan.FromSeconds(0.0667))
            .SetRepeatTimes(-1)
            .SetLauncherType(TaskSource.ELauncherType.FixedUpdate)
            .Run();
        }
    }

    public void StopCombat()
    {
        TargetFrameID = -1;
        FrameID = 0;
        _missOperates.Clear();
        _operates.Clear();
        InCombat = false;
        CanOperate.Value = false;
        GameEntry.Task.CancelTask(ref _testModelTaslID);
    }

    void ProcressOperate()
    {
        if (_missOperates.Count > 0)
        {
            CanOperate.Value = false;
            ProcressMissOperate(_missOperates);
        }
        else if (_operates.Count > 0)
        {
            GS2U_Operate msg = _operates.Peek();
            if (msg.FrameID == FrameID)
            {
                CanOperate.Value = true;
                ProcressMissOperate(_operates);
            }
            else
            {
                CanOperate.Value = false;
            }
        }
        else
        {
            CanOperate.Value = true;
        }
    }

    void ProcressMissOperate(Queue<GS2U_Operate> que)
    {
        int count = 0;
        while (que.Count > 0)
        {
            GS2U_Operate msg = que.Dequeue();
            for (int i = 0; i < msg.Operates.Count; i++)
            {
                OperateSystem.Input(msg.Operates[i]);
            }
            FrameID++;

            ++count;
            if (count > 20)
            {
                break;
            }
        }
    }

    private void OnMissOperate(GS2U_MissOperate msg)
    {
        for (int i = 0; i < msg.Operates.Count; i++)
        {
            OnOperate(msg.Operates[i]);
        }
    }

    private void OnOperate(GS2U_Operate operate)
    {
        // 如果最大帧ID大于当前下发的帧ID 说明是丢失的帧
        if (TargetFrameID > operate.FrameID)
        {
            _missOperates.Enqueue(operate);
        }
        else if (TargetFrameID < operate.FrameID)
        {
            // 说明丢帧 需要向服务拉取丢失的帧
            if (operate.FrameID != TargetFrameID + 1)
            {
                int startIndex = TargetFrameID + 1;
                int count = Mathf.Clamp(operate.FrameID - TargetFrameID, 0, operate.FrameID);
                U2GS_MissOperate msg = new U2GS_MissOperate
                {
                    StartID = startIndex,
                    Count = count
                };

                GameEntry.Net.SendMsg(msg);
                Debug.Log($"丢帧,StartID: {startIndex} 数量: {count}");
            }

            _operates.Enqueue(operate);
            TargetFrameID = operate.FrameID;
        }
        else
        {
            Debug.LogError("收到重复的帧信息");
        }

        LogicUpdate(new FixInt(0.0667));
    }
}