using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class ClassNameGenerator
{
    public static string Generate(string scriptName)
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("[System.Serializable]");
        sb.AppendLine($"public class DT{scriptName} : IDataTable");

        return sb.ToString();
    }
}
