using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BuffExcutor : IBuffExcutor
{
    public IDamageMarkFactory DamageMarkFactory => MVCContainer.Get<IDamageMarkFactory>();

    void IBuffExcutor.Interrupt(Buff buff, int id, int[] parms)
    {
        OnInterrupt(buff, id, parms);
    }

    void IBuffExcutor.Over(Buff buff, int id, int[] parms)
    {
        OnOver(buff, id, parms);
    }

    void IBuffExcutor.Start(Buff buff, int id, int[] parms)
    {
        OnStart(buff, id, parms);
    }

    void IBuffExcutor.Update(Buff buff, int id, int[] parms)
    {
        OnUpdate(buff, id, parms);
    }

    /// <summary>
    /// 被驱散
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="id">excutor编号,使用黑板时使用</param>
    /// <param name="parms">用户数据</param>
    protected virtual void OnInterrupt(Buff buff, int id, int[] parms) { }

    /// <summary>
    /// 结束
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="id">excutor编号,使用黑板时使用</param>
    /// <param name="parms">用户数据</param>
    protected virtual void OnOver(Buff buff, int id, int[] parms) { }

    /// <summary>
    /// 开始添加
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="id">excutor编号,使用黑板时使用</param>
    /// <param name="parms">用户数据</param>
    protected virtual void OnStart(Buff buff, int id, int[] parms) { }

    /// <summary>
    /// 轮询
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="id">excutor编号,使用黑板时使用</param>
    /// <param name="parms">用户数据</param>
    protected virtual void OnUpdate(Buff buff, int id, int[] parms) { }
}