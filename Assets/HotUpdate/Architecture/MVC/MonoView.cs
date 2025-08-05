using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoView : MonoBehaviour
{
    protected virtual void Awake()
    {
        MVCContainer.Inject(this);
    }
}
