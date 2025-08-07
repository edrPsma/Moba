///////////////////////////////////
///该代码由工具自动生成，请勿修改////
///////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class DTHero : IDataTable
{
	string IDataTable.Location => "Hero";
	[ShowInInspector] public readonly int ID;// ID
	[ShowInInspector] public readonly string Name;// 名字
	[ShowInInspector] public readonly string Icon;// 图标
	[ShowInInspector] public readonly int[] ShowSkills;// 展示技能
	[ShowInInspector] public readonly string Model;// 模型
	[ShowInInspector] public readonly string Pic;// 立绘
	[ShowInInspector] public readonly string Asset;// 战斗资源
	[ShowInInspector] public readonly int[] Skills;// 技能
}
