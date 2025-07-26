using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class CustomGenerator
{
    public static string Generate(string scriptName, ExcelInfo excelInfo)
    {
        StringBuilder sb = new StringBuilder("\t");

        string jsonName = ExcelLoaderUtils.BigHump(excelInfo.Name);
        sb.Append($"string IDataTable.Location => \"{jsonName}\";");
        sb.AppendLine();

        return sb.ToString();
    }
}
