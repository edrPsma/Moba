using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModel
{
    void Initialize();
}

public abstract class AbstractModel : IModel
{
    void IModel.Initialize()
    {
        OnInitialize();
    }

    protected virtual void OnInitialize()
    {

    }
}