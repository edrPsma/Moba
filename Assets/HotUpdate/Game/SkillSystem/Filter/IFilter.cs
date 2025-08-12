using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFilter
{
    void Check(List<LogicActor> list, LogicActor owner);

    /// <summary>
    /// 获取第一个符合筛选条件的目标对象
    /// </summary>
    /// <param name="list">筛选对象合集</param>
    /// <returns>若不存在则返回null</returns>
    LogicActor GetFirstTarget(List<LogicActor> list, LogicActor owner);

    void Check(LogicActor[] list, LogicActor owner);
}
