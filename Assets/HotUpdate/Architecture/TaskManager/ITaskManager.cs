using System;
using System.Collections;
using System.ComponentModel;
using Task;
using UnityEngine;

public interface ITaskManager
{
    /// <summary>
    /// 添加任务,参数为任务执行次数
    /// </summary>
    /// <param name="task">任务</param>
    /// <returns></returns>
    ITaskSource AddTask(Action<TaskInfo> task);

    /// <summary>
    /// 取消任务
    /// </summary>
    /// <param name="taskId">任务Id</param>
    void CancelTask(ref int taskId);

    /// <summary>
    /// 取消任务
    /// </summary>
    /// <param name="taskId">任务Id</param>
    void CancelTask(int taskId);

    /// <summary>
    /// 暂停任务
    /// </summary>
    /// <param name="taskId">任务Id</param>
    void PauseTask(int taskId);

    /// <summary>
    /// 恢复任务
    /// </summary>
    /// <param name="taskId">任务Id</param>
    void ResumeTask(int taskId);

    /// <summary>
    /// 是否存在任务
    /// </summary>
    /// <param name="taskId">任务Id</param>
    /// <returns></returns>
    bool ExistTask(int taskId);
}
