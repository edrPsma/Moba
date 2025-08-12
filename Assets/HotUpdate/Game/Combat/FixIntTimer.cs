using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Pool;
using UnityEngine;

public class FixIntTimer
{
    const int MAXITERATIONS = 1000;
    Dictionary<int, TaskSource> _taskDic;
    ObjectPool<TaskSource> _taskInfoPool;
    List<TaskSource> _addTaskCache;
    List<int> _removeList;
    int _newtaskId = 0;

    public FixIntTimer()
    {
        _taskDic = new Dictionary<int, TaskSource>();
        _addTaskCache = new List<TaskSource>();
        _removeList = new List<int>();
        _taskInfoPool = new ObjectPool<TaskSource>(EPoolType.Scalable, 30);
        _taskInfoPool.OnReleaseEvent += taskInfo => taskInfo.Reset();
    }

    public void Tick(FixInt deltaTime)
    {
        ExecuteTasks(deltaTime);
    }

    void ExecuteTasks(FixInt deltaTime)
    {
        AddPendingTasks();

        foreach (var item in _taskDic)
        {
            var task = item.Value;

            if (!task.Active) continue;

            UpdateTaskTimers(task, deltaTime);

            if (task.ExcuteTime == 0)
            {
                task.TaskAction?.Invoke(new TaskInfo(task.TaskID, ++task.ExcuteTime));
            }
            else if (task.ExcuteTime <= task.RepeatTimes || task.RepeatTimes < 0)
            {
                if (task.Timer >= task.DelayTime)
                {
                    ++task.ExcuteTime;
                    task.TaskAction?.Invoke(new TaskInfo(task.TaskID, task.ExcuteTime));
                    task.Timer -= task.DelayTime;
                }
            }

            if (ShouldRemoveTask(task))
            {
                _removeList.Add(task.TaskID);
            }
        }

        RemoveCompletedTasks();
    }

    void AddPendingTasks()
    {
        for (int i = _addTaskCache.Count - 1; i >= 0; i--)
        {
            _taskDic.Add(_addTaskCache[i].TaskID, _addTaskCache[i]);
            _addTaskCache.RemoveAt(i);
        }
    }

    void UpdateTaskTimers(TaskSource task, FixInt deltaTime)
    {
        task.Timer += deltaTime * 1000 * task.RunSpeed();
        task.TotalTimer += deltaTime * 1000 * task.RunSpeed();
    }

    bool ShouldRemoveTask(TaskSource task)
    {
        if (task.Duration <= 0)
        {
            return task.ExcuteTime > task.RepeatTimes && task.RepeatTimes >= 0;
        }
        else
        {
            return task.TotalTimer >= task.Duration;
        }
    }

    void RemoveCompletedTasks()
    {
        foreach (var taskId in _removeList)
        {
            if (_taskDic.ContainsKey(taskId))
            {
                _taskInfoPool.Release(_taskDic[taskId]);
                _taskDic.Remove(taskId);
            }
        }
        _removeList.Clear();
    }

    public TaskSource AddTask(Action<TaskInfo> task)
    {
        TaskSource info = _taskInfoPool.SpawnByType();
        info.TaskAction = task;
        GetUniqueId();
        info.TaskID = _newtaskId;
        _addTaskCache.Add(info);
        return info;
    }

    void GetUniqueId()
    {
        int count = 0;
        while (count < MAXITERATIONS)
        {
            ++_newtaskId;
            if (!_taskDic.ContainsKey(_newtaskId) && _newtaskId != 0)
            {
                return;
            }
            ++count;
        }
        Debug.LogWarning("Too many parallel tasks, maximum number of iterations exceeded");
    }

    public void CancelTask(ref int taskId)
    {
        CancelTaskInternal(taskId);
        taskId = 0;
    }

    public void CancelTask(int taskId)
    {
        CancelTaskInternal(taskId);
    }

    void CancelTaskInternal(int taskId)
    {
        if (_taskDic.ContainsKey(taskId))
        {
            _removeList.Add(taskId);
        }
    }

    public void PauseTask(int taskId)
    {
        if (_taskDic.ContainsKey(taskId))
        {
            TaskSource info = _taskDic[taskId];
            info.Active = false;
        }
    }

    public void ResumeTask(int taskId)
    {
        if (_taskDic.ContainsKey(taskId))
        {
            TaskSource info = _taskDic[taskId];
            info.Active = true;
        }
    }

    public bool ExistTask(int taskId)
    {
        return _taskDic.ContainsKey(taskId);
    }

    public void Dispose()
    {
        _addTaskCache.Clear();
        foreach (var item in _taskDic)
        {
            _taskInfoPool.Release(item.Value);
        }
        _taskDic.Clear();
        _removeList.Clear();
    }

    public class TaskSource
    {
        public int TaskID;
        public bool Active;
        public string Name;
        public FixInt Timer;
        public FixInt TotalTimer;
        public uint ExcuteTime;
        public FixInt DelayTime;
        public FixInt Duration;
        public int RepeatTimes;
        public Func<FixInt> RunSpeed = GetDefultRunSpeed;

        public Action<TaskInfo> TaskAction;
        static FixInt GetDefultRunSpeed() => 1;

        public void Reset()
        {
            TaskID = 0;
            Active = false;
            Timer = 0;
            RunSpeed = GetDefultRunSpeed;
            TotalTimer = 0;
            ExcuteTime = 0;
            DelayTime = 0;
            Duration = 0;
            RepeatTimes = 0;
            Name = string.Empty;
        }

        public TaskSource SetRepeatTimes(int times)
        {
            RepeatTimes = times;
            return this;
        }

        public TaskSource Delay(int milliseconds)
        {
            DelayTime = milliseconds;
            return this;
        }

        public TaskSource Delay(TimeSpan timeSpan)
        {
            DelayTime = new FixInt(timeSpan.TotalMilliseconds);
            return this;
        }

        public TaskSource SetDuration(int milliseconds)
        {
            Duration = milliseconds;
            return this;
        }

        public TaskSource SetDuration(TimeSpan timeSpan)
        {
            Duration = new FixInt(timeSpan.TotalMilliseconds);
            return this;
        }


        public TaskSource SetSpeed(float speed)
        {
            RunSpeed = () => speed;
            return this;
        }

        public TaskSource SetSpeed(Func<FixInt> speedFun)
        {
            if (speedFun == null)
            {
                return this;
            }
            RunSpeed = speedFun;
            return this;
        }

        public TaskSource SetName(string name)
        {
            Name = name;
            return this;
        }

        public int Run()
        {
            Active = true;
            return TaskID;
        }
    }

    public readonly struct TaskInfo
    {
        public readonly int TaskID;
        public readonly uint Times;

        public TaskInfo(int taskID, uint times)
        {
            TaskID = taskID;
            Times = times;
        }
    }
}
