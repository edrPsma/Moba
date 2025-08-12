using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseSelector : ISelector
{
    public IPhysicsSystem PhysicsSystem => MVCContainer.Get<IPhysicsSystem>();

    public abstract void DebugDamageArea(SkillInfo info, ISkillExcutor skillExcutor);

    public abstract void Select(SkillInfo info, ISkillExcutor skillExcutor, List<LogicActor> infos);
}
