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

    /// <summary>
    /// 协程用,避免频繁创建而产生gc,使用迭代形式 最小单位为0.1f
    /// </summary>
    /// <param name="second"></param>
    /// <returns></returns>
    IEnumerator WaitForSeconds(float second);

    /// <summary>
    /// 协程用,避免频繁创建而产生gc
    /// </summary>
    /// <returns></returns>
    WaitForEndOfFrame WaitForEndOfFrame();
    /// <summary>
    /// 在此接口对象上跑协程
    /// </summary>
    /// <param name="methodName"></param>
    /// <returns></returns>
    Coroutine RunCoroutine(string methodName);
    Coroutine RunCoroutine(IEnumerator routine);
    Coroutine RunCoroutine(string methodName, [DefaultValue("null")] object value);
    void EndCoroutine(Coroutine routine);
}
