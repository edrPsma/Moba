using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityCodeGen;
using UnityEditor;
using UnityEngine;

namespace UnityCodeGen.New
{
    [Generator]
    public class MVCCodeGen : ICodeGenerator
    {
        public void Execute(GeneratorContext context)
        {
            string model = Gen<ModelAttribute, IModel>();
            string controller = Gen<ControllerAttribute, IController>();

            string code =
    $@"
// 这个文件由代码生成器自动生成
using UnityEngine;
using Zenject;


public partial class MVCContainer
{{
    static partial void BindController()
    {{
{controller}
    }}

    static partial void BindModel()
    {{
{model}
    }}

}}";

            context.AddCode("Assets/HotUpdate/Architecture/MVC", $"MVCContainer.Register.cs", code);
        }

        static string Gen<TAttribute, I>() where TAttribute : ModuleAttribute
        {
            List<Type> allController = new List<Type>();
            var maskController = TypeCache.GetTypesWithAttribute<TAttribute>();
            foreach (var item in maskController)
            {
                if (!typeof(I).IsAssignableFrom(item)) continue;

                allController.Add(item);
            }

            StringBuilder sb = new StringBuilder();
            foreach (var item in allController)
            {
                TAttribute attribute = item.GetCustomAttribute<TAttribute>();
                if (attribute.Types == null || attribute.Types.Length == 0)
                {
                    sb.AppendLine($"\t\t_container.Bind<{item.FullName}>().AsSingle();");
                }
                else
                {
                    sb.Append("\t\t_container.Bind(");
                    for (int i = 0; i < attribute.Types.Length; i++)
                    {
                        if (i != attribute.Types.Length - 1)
                        {

                            sb.Append($"typeof({attribute.Types[i].FullName}),");
                        }
                        else
                        {
                            sb.Append($"typeof({attribute.Types[i].FullName})");
                        }
                    }
                    sb.Append($").To<{item.FullName}>()");
                    sb.Append(".AsSingle();\r\n");
                }
            }

            return sb.ToString();
        }
    }
}