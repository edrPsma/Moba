using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffAttribute : ReflectionAttribute
{
    public BuffAttribute(EBuffExcutorType key) : base(typeof(IBuffExcutor), key, 100)
    {
    }
}
