using System;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class ReflectionAttribute : Attribute
{
    public Type BaseType;
    public object Key;
    public int Priority = 0;

    public ReflectionAttribute(Type baseType, object key, int priority = 100)
    {
        BaseType = baseType;
        Key = key;
        Priority = priority;
    }

    public virtual void OnReflect(object target)
    {

    }
}