using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttribute
{
    /// <summary>
    /// 最小值
    /// </summary>
    long Min { get; set; }

    /// <summary>
    /// 最大值
    /// </summary>
    long Max { get; set; }

    /// <summary>
    /// 当前值
    /// </summary>
    long Value { get; set; }

    /// <summary>
    /// 能否修改
    /// </summary>
    bool CanModifly { get; set; }

    /// <summary>
    /// 订阅值变化事件
    /// </summary>
    /// <param name="onValueChange">值变化回调</param>
    /// <param name="runInFirst">一开始执行</param>
    void Subscribe(Action<long> onValueChange, bool runInFirst = true);

    /// <summary>
    /// 取消订阅值变化事件
    /// </summary>
    /// <param name="onValueChange"></param>
    void Unsubscribe(Action<long> onValueChange);

    /// <summary>
    /// 取消订阅所有值变化事件
    /// </summary>
    void UnsubscribeAll();
}