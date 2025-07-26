using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityCodeGen;
using UnityEngine;

[Generator]
public class DataTableCodeGen : ICodeGenerator
{
    public void Execute(GeneratorContext context)
    {
        StringBuilder sb = new StringBuilder();

        int count = 0;

        Assembly assembly = typeof(DataTable).Assembly;
        Type[] types = assembly.GetTypes();
        Type idata = typeof(IDataTable);

        List<Type> list = new List<Type>();
        for (int i = 0; i < types.Length; i++)
        {
            if (!idata.IsAssignableFrom(types[i]) || types[i].IsAbstract) continue;

            list.Add(types[i]);
            count++;
        }

        sb.AppendLine($"\t\t_totalCount = {count};");
        for (int i = 0; i < list.Count; i++)
        {
            sb.AppendLine($"\t\tLoad<{list[i].FullName}>();");
        }

        string code =
$@"
// 这个文件由代码生成器自动生成
using System;
using System.Collections.Generic;
using System.Reflection;


public static partial class DataTable
{{
    static partial void Register()
    {{
{sb}
    }}
}}";

        context.AddCode("Assets/HotUpdate/DataTable", $"DataTable.Register.cs", code);
    }
}
