using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    public interface IRegisterations
    {
        int Count { get; }

        void Add(object action, object userData);

        void Remove(object action, object userData);

        TAction GetAction<TAction>(object userData) where TAction : class;
    }
}