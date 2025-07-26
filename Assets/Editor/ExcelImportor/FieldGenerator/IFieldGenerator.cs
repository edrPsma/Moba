using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFieldGenerator
{
    string Generate(string type, string name, string desc);
}
