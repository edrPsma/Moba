using System.Collections;
using System.Collections.Generic;
using Drawing;
using OBB;
using UnityEngine;

public abstract class Obstacle : MonoBehaviourGizmos
{
    protected virtual void Awake()
    {
        MVCContainer.Inject(this);
    }
}
