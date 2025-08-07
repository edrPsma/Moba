using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderingActor : MonoBehaviour
{
    public LogicActor LogicActor { get; private set; }

    public virtual void Initialize(LogicActor logicActor)
    {
        LogicActor = logicActor;
    }
}
