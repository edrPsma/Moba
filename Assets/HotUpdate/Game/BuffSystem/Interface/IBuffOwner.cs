using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public interface IBuffOwner
{
    /// <summary>
    /// 单位
    /// </summary>
    LogicActor Owner { get; }

    /// <summary>
    /// 移除Buff
    /// </summary>
    /// <param name="buff"></param>
    void RemoveBuff(Buff buff);

    /// <summary>
    /// 移除Buff
    /// </summary>
    /// <param name="buffID"></param>
    void RemoveBuff(int buffID);

    /// <summary>
    /// 添加Buff
    /// </summary>
    /// <param name="buffId">buffID</param>
    /// <param name="caster">施法者</param>
    /// <param name="skillID">技能ID</param>
    void AddBuff(int buffId, LogicActor caster, int skillID);

    /// <summary>
    /// 驱散buff
    /// </summary>
    /// <param name="buffID"></param>
    void Disperse(int buffID);

    /// <summary>
    /// 获取所有buffId
    /// </summary>
    /// <returns></returns>
    int[] GetAllBuffId();

    /// <summary>
    /// 获取所有Buff
    /// </summary>
    /// <returns></returns>
    Buff[] GetAllBuff();

    /// <summary>
    /// 获取Buff
    /// </summary>
    /// <returns></returns>
    Buff GetBuff(int buffId);

    /// <summary>
    /// 逻辑帧执行
    /// </summary>
    /// <param name="deltaTime"></param>
    void LogicUpdate(FixInt deltaTime);

    void Dispose();
}
