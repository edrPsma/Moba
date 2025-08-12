using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityCodeGen;
using UnityEditor;
using UnityEngine;

[Generator]
public class ReflectionMangerCodeGen : ICodeGenerator
{
    public void Execute(GeneratorContext context)
    {
        List<ReflectionInfo> infoList = new List<ReflectionInfo>();
        HashSet<Type> baseTypeList = new HashSet<Type>();

        var list = TypeCache.GetTypesWithAttribute<ReflectionAttribute>();
        foreach (var item in list)
        {
            ReflectionAttribute attribute = item.GetCustomAttribute<ReflectionAttribute>();
            object key = attribute.Key ?? item;
            infoList.Add(new ReflectionInfo { BaseType = attribute.BaseType, Key = key, Priority = attribute.Priority, Target = item, ReflectionAttribute = attribute });
            baseTypeList.Add(attribute.BaseType);
        }

        infoList.Sort(Sort);

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("\t\t\t// InitDic");
        InitDic(baseTypeList, sb);

        sb.AppendLine();
        sb.AppendLine();
        sb.AppendLine();

        sb.AppendLine("\t\t\t// Register");
        for (int i = 0; i < infoList.Count; i++)
        {
            BindTarget(infoList[i], sb);
            sb.AppendLine();
        }

        string code =
$@"
// 这个文件由代码生成器自动生成
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Reflection
{{
    public class ReflectionHandler:IReflectionHandler
    {{
        public Dictionary<Type, Dictionary<object, object>> RefDic {{get;}}
        public Dictionary<Type, Dictionary<object, Delegate>> DelegateDic {{get;}}

		public ReflectionHandler()
		{{
			RefDic = new Dictionary<Type, Dictionary<object, object>>();
			DelegateDic = new Dictionary<Type, Dictionary<object, Delegate>>();
		}}

        public void Register()
        {{
{sb}
        }}
    }}
}}";

        for (int i = 0; i < infoList.Count; i++)
        {
            BindTarget(infoList[i], sb);
        }

        context.AddCode("Assets/HotUpdate/Architecture/Reflection/", $"ReflectionHandler.cs", code);
    }

    private int Sort(ReflectionInfo x, ReflectionInfo y)
    {
        if (x.Priority > y.Priority)
        {
            return 1;
        }
        else if (x.Priority == y.Priority)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    void InitDic(HashSet<Type> list, StringBuilder builder)
    {
        foreach (var item in list)
        {
            builder.AppendLine($"\t\t\tRefDic.Add(typeof({item}), new Dictionary<object, object>());");
            builder.AppendLine($"\t\t\tDelegateDic.Add(typeof({item}), new Dictionary<object, Delegate>());");
            builder.AppendLine();
        }
    }

    void BindTarget(ReflectionInfo info, StringBuilder builder)
    {
        string attName = $"{info.Target.ToString().Replace(".", "_")}_Attr";
        string instanceName = $"{info.Target.ToString().Replace(".", "_")}_Ins";
        builder.AppendLine($"\t\t\tReflectionAttribute {attName} = typeof({info.Target}).GetCustomAttribute<ReflectionAttribute>();");
        builder.AppendLine($"\t\t\t{info.Target} {instanceName} = new {info.Target}();");
        builder.AppendLine($"\t\t\t{attName}.OnReflect({instanceName});");
        builder.AppendLine($"\t\t\tRefDic[{attName}.BaseType].SetValue({attName}.Key, {instanceName});");

        // 收集方法
        MethodInfo[] methodInfos = info.Target.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        for (int i = 0; i < methodInfos.Length; i++)
        {
            var cur = methodInfos[i];

            ReflectionFunAttribute attribute = cur.GetCustomAttribute<ReflectionFunAttribute>();

            if (attribute == null) continue;


            string funcName = $"{instanceName}_{methodInfos[i].Name}";
            string funcAtt = $"{instanceName}_{methodInfos[i].Name}_Att";
            builder.AppendLine($"\t\t\tMethodInfo {funcName}_Info = typeof({info.Target}).GetMethod(\"{methodInfos[i].Name}\",BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);");
            builder.AppendLine($"\t\t\tReflectionFunAttribute {funcAtt} = {funcName}_Info.GetCustomAttribute<ReflectionFunAttribute>();");
            builder.AppendLine($"\t\t\tDelegate {funcName} = {funcName}_Info.CreateDelegate({funcAtt}.DelegateType, {instanceName});");
            builder.AppendLine($"\t\t\tDelegateDic[{attName}.BaseType].SetValue({funcAtt}.Key, {funcName});");
        }
    }

    class ReflectionInfo
    {
        public Type BaseType;
        public object Key;
        public int Priority;
        public Type Target;
        public ReflectionAttribute ReflectionAttribute;
    }
}
