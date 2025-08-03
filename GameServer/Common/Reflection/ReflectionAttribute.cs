using System;
using System.Collections.Generic;

namespace GameServer.Common
{
    [AttributeUsage(AttributeTargets.Class)]
    public class ReflectionAttribute : Attribute
    {
        public List<Type> Types;

        public ReflectionAttribute(params Type[] types)
        {
            Types = new List<Type>();
            Types.AddRange(types);
        }
    }
}