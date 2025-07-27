using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public partial class MVCContainer
{
    static DiContainer _container = new DiContainer();

    static MVCContainer()
    {
        BindModel();

        BindController();
    }

    static partial void BindModel();

    static partial void BindController();

    public static void Inject(object injectable)
    {
        _container.Inject(injectable);
    }

    public static T NewAndInject<T>() where T : new()
    {
        T t = new T();
        Inject(t);

        return t;
    }
}
