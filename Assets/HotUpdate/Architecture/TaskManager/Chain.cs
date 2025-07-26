using System;
using System.Collections;
using System.Collections.Generic;
using Task;
using UnityEngine;

public partial class TaskSource
{
    public ITaskSource SetRepeatTimes(int times)
    {
        RepeatTimes = times;
        return this;
    }

    public ITaskSource DelayFrame(int frameCount)
    {
        DelayTime = frameCount;
        DelayType = ETimeType.Frame;
        return this;
    }

    public ITaskSource Delay(int milliseconds)
    {
        DelayTime = milliseconds;
        DelayType = ETimeType.Millisecond;
        return this;
    }

    public ITaskSource Delay(TimeSpan timeSpan)
    {
        DelayTime = (long)timeSpan.TotalMilliseconds;
        DelayType = ETimeType.Millisecond;
        return this;
    }

    public ITaskSource SetFrameDuration(int frameCount)
    {
        Duration = frameCount;
        DurationType = ETimeType.Frame;
        return this;
    }

    public ITaskSource SetDuration(int milliseconds)
    {
        Duration = milliseconds;
        DurationType = ETimeType.Millisecond;
        return this;
    }

    public ITaskSource SetDuration(TimeSpan timeSpan)
    {
        Duration = (long)timeSpan.TotalMilliseconds;
        DurationType = ETimeType.Millisecond;
        return this;
    }

    public ITaskSource IgnoreScale(bool ignore)
    {
        UnScale = ignore;
        return this;
    }

    public ITaskSource RunInFirst(bool runInFirst)
    {
        IfRunInFirst = runInFirst;
        return this;
    }

    public ITaskSource OnStart(Action onStart)
    {
        StartAction += onStart;
        return this;
    }

    public ITaskSource OnUpdate(Action onUpdate)
    {
        UpdateAction += onUpdate;
        return this;
    }

    public ITaskSource OnComplete(Action onComplete)
    {
        CompleteAction += onComplete;
        return this;
    }

    public ITaskSource OnPause(Action onPause)
    {
        PauseAction += onPause;
        return this;
    }

    public ITaskSource OnResume(Action onResume)
    {
        ResumeAction += onResume;
        return this;
    }

    public ITaskSource OnCancel(Action onCancel)
    {
        CancelAction += onCancel;
        return this;
    }

    public ITaskSource SetSpeed(float speed)
    {
        RunSpeed = () => speed;
        return this;
    }

    public ITaskSource SetSpeed(Func<float> speedFun)
    {
        if (speedFun == null)
        {
            return this;
        }
        RunSpeed = speedFun;
        return this;
    }

    public ITaskSource SetName(string name)
    {
        Name = name;
        return this;
    }

    public ITaskSource SetLauncherType(ELauncherType launcherType)
    {
        LauncherType = launcherType;
        return this;
    }

    public int Run()
    {
        Active = true;
        return taskId;
    }
}