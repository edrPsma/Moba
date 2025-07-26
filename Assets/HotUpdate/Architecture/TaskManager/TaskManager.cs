using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Pool;
using Sirenix.OdinInspector;
using Template;
using UnityEngine;

namespace Task
{
    public class TaskManager : MonoSingleton<ITaskManager, TaskManager>, ITaskManager
    {
        const float WAIT_FOR_SECOND_STEP = 0.1f;
        const int MAXITERATIONS = 1000;
        [ShowInInspector] Dictionary<int, TaskSource> _taskDic;
        ObjectPool<TaskSource> _taskInfoPool;
        List<TaskSource> _addTaskCache;
        List<int> _removeList;
        int _newtaskId = 0;
        WaitForSeconds _waitForSeconds;
        WaitForEndOfFrame _waitForEndOfFrame;

        protected override void OnInit()
        {
            DontDestroyOnLoad(gameObject);
            _taskDic = new Dictionary<int, TaskSource>();
            _addTaskCache = new List<TaskSource>();
            _removeList = new List<int>();
            _taskInfoPool = new ObjectPool<TaskSource>(EPoolType.Scalable, 30);
            _taskInfoPool.OnReleaseEvent += taskInfo => taskInfo.Reset();

            _waitForSeconds = new WaitForSeconds(WAIT_FOR_SECOND_STEP);
            _waitForEndOfFrame = new WaitForEndOfFrame();
        }

        private void LateUpdate()
        {
            ExecuteTasks(TaskSource.ELauncherType.LateUpdate, Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void FixedUpdate()
        {
            ExecuteTasks(TaskSource.ELauncherType.FixedUpdate, Time.fixedDeltaTime, Time.fixedUnscaledDeltaTime);
        }

        private void Update()
        {
            ExecuteTasks(TaskSource.ELauncherType.Update, Time.deltaTime, Time.unscaledDeltaTime);
        }

        void ExecuteTasks(TaskSource.ELauncherType launcherType, float scaleTime, float unscaleTime)
        {
            AddPendingTasks(launcherType);

            foreach (var item in _taskDic)
            {
                var task = item.Value;

                if (task.LauncherType != launcherType || !task.Active) continue;

                UpdateTaskTimers(task, scaleTime, unscaleTime);
                task.StartAction?.Invoke();
                task.StartAction = null;

                if (task.IfRunInFirst && task.ExcuteTime == 0)
                {
                    task.TaskAction?.Invoke(new TaskInfo(task.TaskId, ++task.ExcuteTime));
                }
                else if (task.ExcuteTime <= task.RepeatTimes || task.RepeatTimes < 0)
                {
                    if (task.Timer >= task.DelayTime)
                    {
                        ++task.ExcuteTime;
                        task.TaskAction?.Invoke(new TaskInfo(task.TaskId, task.ExcuteTime));
                        task.Timer -= task.DelayTime;
                    }
                }

                task.UpdateAction?.Invoke();

                if (ShouldRemoveTask(task))
                {
                    task.CompleteAction?.Invoke();
                    _removeList.Add(task.TaskId);
                }
            }

            RemoveCompletedTasks();
        }

        void AddPendingTasks(TaskSource.ELauncherType launcherType)
        {
            for (int i = _addTaskCache.Count - 1; i >= 0; i--)
            {
                if (_addTaskCache[i].LauncherType != launcherType) continue;

                _taskDic.Add(_addTaskCache[i].TaskId, _addTaskCache[i]);
                _addTaskCache.RemoveAt(i);
            }
        }

        void UpdateTaskTimers(TaskSource task, float scaleTime, float unscaleTime)
        {
            if (task.DelayType == ETimeType.Frame)
            {
                task.Timer += 1 * task.RunSpeed();
            }
            else if (task.DelayType == ETimeType.Millisecond)
            {
                task.Timer += (task.UnScale ? unscaleTime : scaleTime) * 1000 * task.RunSpeed();
            }

            if (task.DurationType == ETimeType.Frame)
            {
                task.TotalTimer += 1 * task.RunSpeed();
            }
            else if (task.DurationType == ETimeType.Millisecond)
            {
                task.TotalTimer += (task.UnScale ? unscaleTime : scaleTime) * 1000 * task.RunSpeed();
            }
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

        public ITaskSource AddTask(Action<TaskInfo> task)
        {
            TaskSource info = _taskInfoPool.SpawnByType();
            info.TaskAction = task;
            GetUniqueId();
            info.taskId = _newtaskId;
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
                TaskSource info = _taskDic[taskId];
                info.CancelAction?.Invoke();
                _removeList.Add(taskId);
            }
        }

        public void PauseTask(int taskId)
        {
            if (_taskDic.ContainsKey(taskId))
            {
                TaskSource info = _taskDic[taskId];
                info.Active = false;
                info.PauseAction?.Invoke();
            }
        }

        public void ResumeTask(int taskId)
        {
            if (_taskDic.ContainsKey(taskId))
            {
                TaskSource info = _taskDic[taskId];
                info.Active = true;
                info.ResumeAction?.Invoke();
            }
        }

        public bool ExistTask(int taskId)
        {
            return _taskDic.ContainsKey(taskId);
        }

        public IEnumerator WaitForSeconds(float second)
        {
            if (second < WAIT_FOR_SECOND_STEP)
            {
                yield return null;
            }
            else
            {
                for (float i = 0; i < second; i += WAIT_FOR_SECOND_STEP)
                {
                    yield return _waitForSeconds;
                }
            }
        }

        public WaitForEndOfFrame WaitForEndOfFrame()
        {
            return _waitForEndOfFrame;
        }

        public Coroutine RunCoroutine(string methodName)
        {
            return StartCoroutine(methodName);
        }

        public Coroutine RunCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }

        public Coroutine RunCoroutine(string methodName, [DefaultValue("null")] object value)
        {
            return StartCoroutine(methodName, value);
        }

        public void EndCoroutine(Coroutine routine)
        {
            if (routine == null) return;

            StopCoroutine(routine);
        }
    }
}

