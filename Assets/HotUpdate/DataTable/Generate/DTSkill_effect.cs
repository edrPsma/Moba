///////////////////////////////////
///该代码由工具自动生成，请勿修改////
///////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class DTSkill_effect : IDataTable
{
	string IDataTable.Location => "Skill_effect";
	[ShowInInspector] public readonly int ID;// ID
	[ShowInInspector] public readonly int Type;// 类型
	[ShowInInspector] public readonly int TriggerTimeing;// 触发时机
	[ShowInInspector] public readonly int[] EffectValue;// 参数
}
