using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttributeSet
{
    /// <summary>
    /// 当前血量
    /// </summary>
    long HP { get; }

    /// <summary>
    /// 血量属性
    /// </summary>
    IAttribute HPAttribute { get; }

    /// <summary>
    /// 护盾属性(这里的护盾纯显示，逻辑在护盾buff)
    /// </summary>
    IAttribute ShieldAttribute { get; }

    /// <summary>
    /// 重置属性
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <param name="value">属性值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    void ResetAttribute(int key, long value, long min, long max);

    /// <summary>
    /// 重置血量
    /// </summary>
    void ResetHP(long value, long min, long max);

    /// <summary>
    /// 重置属性
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <param name="value">属性值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    void ResetAttribute(EAttributeKey key, long value, long min, long max);

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <param name="value">属性值</param>
    void SetValue(int key, long value);

    /// <summary>
    /// 设置属性值
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <param name="value">属性值</param>
    void SetValue(EAttributeKey key, long value);

    /// <summary>
    /// 增加属性值
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <param name="addValue">增加的属性值</param>
    void AddValue(EAttributeKey key, long addValue);

    /// <summary>
    /// 增加HP
    /// </summary>
    /// <param name="addValue"></param>
    void AddHP(LogicActor source, long addValue);

    /// <summary>
    /// 增加护盾(这里的护盾纯显示，逻辑在护盾buff)
    /// </summary>
    /// <param name="addValue"></param>
    void AddShield(long addValue);

    /// <summary>
    /// 增加属性值
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <param name="addValue">增加的属性值</param>
    void AddValue(int key, long addValue);

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <returns>属性值</returns>
    long GetValue(int key);

    /// <summary>
    /// 获取属性值
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <returns>属性值</returns>
    long GetValue(EAttributeKey key);

    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <returns>属性</returns>
    IAttribute GetAttribute(int key);

    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="key">属性键值</param>
    /// <returns>属性</returns>
    IAttribute GetAttribute(EAttributeKey key);
}