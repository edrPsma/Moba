using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Sirenix.OdinInspector;
using Task;
using UnityEngine;

public partial class TaskSource : ITaskSource
{
    /// <summary>
    /// 任务ID
    /// </summary>
    internal int taskId;

    /// <summary>
    /// 任务ID
    /// </summary>
    public int TaskId => taskId;

    [ShowInInspector] internal string Name;

    internal ELauncherType LauncherType;

    /// <summary>
    /// 是否激活
    /// </summary>
    internal bool Active;

    /// <summary>
    /// 计时器
    /// </summary>
    internal float Timer;

    /// <summary>
    /// 总时间计时器
    /// </summary>
    internal float TotalTimer;

    /// <summary>
    /// 执行次数
    /// </summary>
    internal uint ExcuteTime;

    /// <summary>
    /// 延时
    /// </summary>
    internal long DelayTime;

    /// <summary>
    /// 延时时间类型
    /// </summary>
    internal ETimeType DelayType;

    /// <summary>
    /// 持续时间
    /// </summary>
    internal long Duration;

    /// <summary>
    /// 持续时间类型
    /// </summary>
    internal ETimeType DurationType;

    /// <summary>
    /// 重复次数
    /// </summary>
    internal int RepeatTimes;

    /// <summary>
    /// 忽略时间缩放
    /// </summary>
    internal bool UnScale;

    /// <summary>
    /// 任务开始时是否立即调用一次
    /// </summary>
    internal bool IfRunInFirst;

    internal Func<float> RunSpeed = GetDefultRunSpeed;

    #region Action
    /// <summary>
    /// 任务委托
    /// </summary>
    internal Action<TaskInfo> TaskAction;

    /// <summary>
    /// 帧委托
    /// </summary>
    internal Action UpdateAction;

    /// <summary>
    /// 结束委托
    /// </summary>
    internal Action CompleteAction;

    /// <summary>
    /// 任务开始委托
    /// </summary>
    internal Action StartAction;

    /// <summary>
    /// 任务取消委托
    /// </summary>
    internal Action CancelAction;

    /// <summary>
    /// 任务暂停委托
    /// </summary>
    internal Action PauseAction;

    /// <summary>
    /// 任务恢复委托
    /// </summary>
    internal Action ResumeAction;
    #endregion

    internal void Reset()
    {
        Active = false;
        Timer = 0;
        RunSpeed = GetDefultRunSpeed;
        TotalTimer = 0;
        ExcuteTime = 0;
        DelayTime = 0;
        DelayType = ETimeType.None;
        Duration = 0;
        DurationType = ETimeType.None;
        RepeatTimes = 0;
        UnScale = false;
        IfRunInFirst = false;
        TaskAction = null;
        UpdateAction = null;
        CompleteAction = null;
        StartAction = null;
        CancelAction = null;
        PauseAction = null;
        ResumeAction = null;
        Name = string.Empty;
    }

    static float GetDefultRunSpeed() => 1;

    public enum ELauncherType
    {
        Update = 0,

        FixedUpdate = 1,

        LateUpdate = 3,
    }
}