using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISelector
{
    /// <summary>
    /// 缓存碰撞器数组
    /// </summary>
    public static LogicActor[] CacheArray = new LogicActor[50];

    /// <summary>
    /// 将在伤害范围内的单位选择出来
    /// </summary>
    /// <param name="info"></param>
    /// <param name="skillExcutor"></param>
    /// <param name="infos"></param>
    void Select(SkillInfo info, ISkillExcutor skillExcutor, List<LogicActor> infos);

    /// <summary>
    /// 绘制伤害范围
    /// </summary>
    /// <param name="info"></param>
    /// <param name="skillExcutor"></param>
    void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor);
}