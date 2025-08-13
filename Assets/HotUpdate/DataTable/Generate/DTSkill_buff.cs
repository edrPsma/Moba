///////////////////////////////////
///该代码由工具自动生成，请勿修改////
///////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class DTSkill_buff : IDataTable
{
	string IDataTable.Location => "Skill_buff";
	[ShowInInspector] public readonly int ID;// ID
	[ShowInInspector] public readonly int Group;// 类型
	[ShowInInspector] public readonly int Effect;// 效果
	[ShowInInspector] public readonly int[] EffectValue;// 参数
	[ShowInInspector] public readonly int Duration;// 持续时间（毫秒）
	[ShowInInspector] public readonly int Interval;// 间隔（毫秒）
	[ShowInInspector] public readonly int Position;// 特效位置1=底部2=身体3=头部
	[ShowInInspector] public readonly string Prefab;// 特效名称
}
