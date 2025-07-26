using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class DefultFieldGenerator : IFieldGenerator
{
    public string Generate(string type, string name, string desc)
    {
        StringBuilder sb = new StringBuilder("\t[ShowInInspector] public readonly ");
        sb.Append("string");
        sb.Append(" ");
        sb.Append(name);
        sb.Append(";");
        sb.Append("// ");
        sb.Append(desc.Replace("\n\r", ""));

        return sb.ToString();
    }
}
