using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public static class ExcelLoaderUtils
{
    public static string BigHump(string str)
    {
        return $"{str.Substring(0, 1).ToUpper()}{str.Substring(1).ToLower()}";
    }

    public static string Format(string str)
    {
        return str.Replace(" ", "")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("-", "")
            .Replace(",", "")
            .Replace("'", "");
    }

    public static string Format2(string str)
    {
        return str.Replace(" ", "_")
            .Replace("\n", "")
            .Replace("\r", "")
            .Replace("-", "")
            .Replace(",", "")
            .Replace("'", "");
    }

    public static bool IsNumeric(string strNumber)
    {
        Regex regex = new Regex(@"^(\-|\+)?\d+(\.\d+)?$");
        return regex.IsMatch(strNumber);
    }

    public static bool IsArray(string type)
    {
        return type.Contains("[]");
    }

    public static string CheckValue(string value, string type)
    {
        if (!string.IsNullOrEmpty(value)) return value;

        if (IsNumeric(value))
        {
            return "0";
        }
        else if (IsArray(type))
        {
            return "[]";
        }
        else
        {
            return "";
        }
    }
}
