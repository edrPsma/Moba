using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffExcutor
{
    /// <summary>
    /// Start
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="id"></param>
    /// <param name="parms"></param>
    void Start(Buff buff, int id, int[] parms);

    /// <summary>
    /// 帧函数
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="id"></param>
    /// <param name="parms"></param>
    void Update(Buff buff, int id, int[] parms);

    /// <summary>
    /// 驱散
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="id"></param>
    /// <param name="parms"></param>
    void Interrupt(Buff buff, int id, int[] parms);

    /// <summary>
    /// 结束
    /// </summary>
    /// <param name="buff"></param>
    /// <param name="id"></param>
    /// <param name="parms"></param>
    void Over(Buff buff, int id, int[] parms);
}