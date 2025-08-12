using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Observable;
using UnityEngine;

public class FixIntVariable : Variable<FixInt>
{
    public FixIntVariable(int value)
    {
        Value = value;
    }
}
