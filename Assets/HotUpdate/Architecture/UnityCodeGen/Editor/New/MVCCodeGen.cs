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
                sb.AppendLine($"\t\t_container.BindInterfacesAndSelfTo<{item.FullName}>().AsSingle();\r\n");
            }

            return sb.ToString();
        }
    }
}