using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITaskSource
{
    int TaskId { get; }

    /// <summary>
    /// 设置重复次数,若为-1则为无线循环
    /// </summary>
    /// <param name="times">次数</param>
    /// <returns></returns>
    ITaskSource SetRepeatTimes(int times);

    /// <summary>
    /// 延时n帧
    /// </summary>
    /// <param name="frameCount">帧数量</param>
    /// <returns></returns>
    ITaskSource DelayFrame(int frameCount);

    /// <summary>
    /// 延时n毫秒
    /// </summary>
    /// <param name="milliseconds">毫秒数</param>
    /// <returns></returns>
    ITaskSource Delay(int milliseconds);

    /// <summary>
    /// 延时
    /// </summary>
    /// <param name="timeSpan">时间</param>
    /// <returns></returns>
    ITaskSource Delay(TimeSpan timeSpan);

    /// <summary>
    /// 设置持续时间（帧）
    /// </summary>
    /// <param name="frameCount">帧数量</param>
    /// <returns></returns>
    ITaskSource SetFrameDuration(int frameCount);

    /// <summary>
    /// 设置持续时间（毫秒）
    /// </summary>
    /// <param name="milliseconds">毫秒</param>
    /// <returns></returns>
    ITaskSource SetDuration(int milliseconds);

    /// <summary>
    /// 设置持续时间
    /// </summary>
    /// <param name="timeSpan">时间</param>
    /// <returns></returns>
    ITaskSource SetDuration(TimeSpan timeSpan);

    /// <summary>
    /// 是否忽略时间缩放
    /// </summary>
    /// <param name="ignore">是否忽略</param>
    /// <returns></returns>
    ITaskSource IgnoreScale(bool ignore);

    /// <summary>
    /// 任务开始时是否立即调用一次
    /// </summary>
    /// <param name="runInFirst"></param>
    /// <returns></returns>
    ITaskSource RunInFirst(bool runInFirst);

    /// <summary>
    /// 注册开始执行时事件
    /// </summary>
    /// <param name="onStart"></param>
    /// <returns></returns>
    ITaskSource OnStart(Action onStart);

    /// <summary>
    /// 注册帧事件
    /// </summary>
    /// <param name="onUpdate"></param>
    /// <returns></returns>
    ITaskSource OnUpdate(Action onUpdate);

    /// <summary>
    /// 注册完成事件
    /// </summary>
    /// <param name="onComplete"></param>
    /// <returns></returns>
    ITaskSource OnComplete(Action onComplete);

    /// <summary>
    /// 注册暂停事件
    /// </summary>
    /// <param name="onPause"></param>
    /// <returns></returns>
    ITaskSource OnPause(Action onPause);

    /// <summary>
    /// 注册恢复事件
    /// </summary>
    /// <param name="onResume"></param>
    /// <returns></returns>
    ITaskSource OnResume(Action onResume);

    /// <summary>
    /// 注册取消事件
    /// </summary>
    /// <param name="onCancel"></param>
    /// <returns></returns>
    ITaskSource OnCancel(Action onCancel);

    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="speed"></param>
    /// <returns></returns>
    ITaskSource SetSpeed(float speed);

    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="speedFun"></param>
    /// <returns></returns>
    ITaskSource SetSpeed(Func<float> speedFun);

    /// <summary>
    /// 设置执行模式
    /// </summary>
    /// <param name="launcherType"></param>
    /// <returns></returns>
    ITaskSource SetLauncherType(TaskSource.ELauncherType launcherType);

    /// <summary>
    /// 设置名字
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    ITaskSource SetName(string name);

    /// <summary>
    /// 执行任务
    /// </summary>
    /// <returns>任务ID</returns>
    int Run();
}
