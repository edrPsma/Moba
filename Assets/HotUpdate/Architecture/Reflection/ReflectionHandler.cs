
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
			RefDic.Add(typeof(IBuffExcutor), new Dictionary<object, object>());
			DelegateDic.Add(typeof(IBuffExcutor), new Dictionary<object, Delegate>());

			RefDic.Add(typeof(ISkillEffector), new Dictionary<object, object>());
			DelegateDic.Add(typeof(ISkillEffector), new Dictionary<object, Delegate>());

			RefDic.Add(typeof(ISkillExcutor), new Dictionary<object, object>());
			DelegateDic.Add(typeof(ISkillExcutor), new Dictionary<object, Delegate>());

			RefDic.Add(typeof(ISelector), new Dictionary<object, object>());
			DelegateDic.Add(typeof(ISelector), new Dictionary<object, Delegate>());




			// Register
			ReflectionAttribute CureBuff_Attr = typeof(CureBuff).GetCustomAttribute<ReflectionAttribute>();
			CureBuff CureBuff_Ins = new CureBuff();
			CureBuff_Attr.OnReflect(CureBuff_Ins);
			RefDic[CureBuff_Attr.BaseType].SetValue(CureBuff_Attr.Key, CureBuff_Ins);

			ReflectionAttribute AddBuffEffector_Attr = typeof(AddBuffEffector).GetCustomAttribute<ReflectionAttribute>();
			AddBuffEffector AddBuffEffector_Ins = new AddBuffEffector();
			AddBuffEffector_Attr.OnReflect(AddBuffEffector_Ins);
			RefDic[AddBuffEffector_Attr.BaseType].SetValue(AddBuffEffector_Attr.Key, AddBuffEffector_Ins);

			ReflectionAttribute FlashEffector_Attr = typeof(FlashEffector).GetCustomAttribute<ReflectionAttribute>();
			FlashEffector FlashEffector_Ins = new FlashEffector();
			FlashEffector_Attr.OnReflect(FlashEffector_Ins);
			RefDic[FlashEffector_Attr.BaseType].SetValue(FlashEffector_Attr.Key, FlashEffector_Ins);

			ReflectionAttribute CommonRule_Attr = typeof(CommonRule).GetCustomAttribute<ReflectionAttribute>();
			CommonRule CommonRule_Ins = new CommonRule();
			CommonRule_Attr.OnReflect(CommonRule_Ins);
			RefDic[CommonRule_Attr.BaseType].SetValue(CommonRule_Attr.Key, CommonRule_Ins);

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