using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IController
{
    void Initialize();
}

public abstract class AbstarctController : IController
{
    void IController.Initialize()
    {
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {

    }
}