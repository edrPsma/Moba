using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public static class NamespaceGenerator
{
    public static string Generate()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("///////////////////////////////////");
        sb.AppendLine("///该代码由工具自动生成，请勿修改////");
        sb.AppendLine("///////////////////////////////////");
        sb.AppendLine("using System.Collections;");
        sb.AppendLine("using System.Collections.Generic;");
        sb.AppendLine("using UnityEngine;");
        sb.AppendLine("using Sirenix.OdinInspector;");

        return sb.ToString();
    }
}
