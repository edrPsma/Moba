
// 这个文件由代码生成器自动生成
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Reflection
{
    public class ReflectionHandler:IReflectionHandler
    {
        public Dictionary<Type, Dictionary<object, object>> RefDic {get;}
        public Dictionary<Type, Dictionary<object, Delegate>> DelegateDic {get;}

		public ReflectionHandler()
		{
			RefDic = new Dictionary<Type, Dictionary<object, object>>();
			DelegateDic = new Dictionary<Type, Dictionary<object, Delegate>>();
		}

        public void Register()
        {
			// InitDic
			RefDic.Add(typeof(ISelector), new Dictionary<object, object>());
			DelegateDic.Add(typeof(ISelector), new Dictionary<object, Delegate>());




			// Register
			ReflectionAttribute OffsetRectangleSelector_Attr = typeof(OffsetRectangleSelector).GetCustomAttribute<ReflectionAttribute>();
			OffsetRectangleSelector OffsetRectangleSelector_Ins = new OffsetRectangleSelector();
			OffsetRectangleSelector_Attr.OnReflect(OffsetRectangleSelector_Ins);
			RefDic[OffsetRectangleSelector_Attr.BaseType].SetValue(OffsetRectangleSelector_Attr.Key, OffsetRectangleSelector_Ins);

			ReflectionAttribute RectangleSelector_Attr = typeof(RectangleSelector).GetCustomAttribute<ReflectionAttribute>();
			RectangleSelector RectangleSelector_Ins = new RectangleSelector();
			RectangleSelector_Attr.OnReflect(RectangleSelector_Ins);
			RefDic[RectangleSelector_Attr.BaseType].SetValue(RectangleSelector_Attr.Key, RectangleSelector_Ins);

			ReflectionAttribute RoundSelector_Attr = typeof(RoundSelector).GetCustomAttribute<ReflectionAttribute>();
			RoundSelector RoundSelector_Ins = new RoundSelector();
			RoundSelector_Attr.OnReflect(RoundSelector_Ins);
			RefDic[RoundSelector_Attr.BaseType].SetValue(RoundSelector_Attr.Key, RoundSelector_Ins);

			ReflectionAttribute SectorSelector_Attr = typeof(SectorSelector).GetCustomAttribute<ReflectionAttribute>();
			SectorSelector SectorSelector_Ins = new SectorSelector();
			SectorSelector_Attr.OnReflect(SectorSelector_Ins);
			RefDic[SectorSelector_Attr.BaseType].SetValue(SectorSelector_Attr.Key, SectorSelector_Ins);

			ReflectionAttribute SingleTargetSelector_Attr = typeof(SingleTargetSelector).GetCustomAttribute<ReflectionAttribute>();
			SingleTargetSelector SingleTargetSelector_Ins = new SingleTargetSelector();
			SingleTargetSelector_Attr.OnReflect(SingleTargetSelector_Ins);
			RefDic[SingleTargetSelector_Attr.BaseType].SetValue(SingleTargetSelector_Attr.Key, SingleTargetSelector_Ins);


        }
    }
}