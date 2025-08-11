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

        InitializeAllControllerAndModel();
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

    static void InitializeAllControllerAndModel()
    {
        List<IModel> models = _container.ResolveAll<IModel>();
        foreach (var item in models)
        {
            item.Initialize();
        }

        List<IController> controllers = _container.ResolveAll<IController>();
        foreach (var item in controllers)
        {
            item.Initialize();
        }
    }

    public static T Get<T>()
    {
        return _container.Resolve<T>();
    }
}
