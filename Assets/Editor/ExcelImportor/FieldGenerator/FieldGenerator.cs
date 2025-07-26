using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FieldGenerator
{
    static Dictionary<string, IFieldGenerator> _dic = new Dictionary<string, IFieldGenerator>
    {
        {"int",new IntGenerator()},
        {"float",new floatGenerator()},
        {"int[]",new IntArrayGenerator()},
        {"int[][]",new Int2ArrayGenerator()},
        {"float[]",new FloatArrayGenerator()},
        {"float[][]",new Float2ArrayGenerator()},
        {"long",new LongGenerator()},
    };

    static IFieldGenerator Defult = new DefultFieldGenerator();

    public static IFieldGenerator GetFieldGenerator(string type)
    {
        if (_dic.ContainsKey(type))
        {
            return _dic[type];
        }
        else
        {
            return Defult;
        }
    }
}
