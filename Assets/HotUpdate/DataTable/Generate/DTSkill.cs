///////////////////////////////////
///该代码由工具自动生成，请勿修改////
///////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class DTSkill : IDataTable
{
	string IDataTable.Location => "Skill";
	[ShowInInspector] public readonly int ID;// ID
	[ShowInInspector] public readonly string Icon;// 图标
	[ShowInInspector] public readonly string Config;// 技能配置
	[ShowInInspector] public readonly int Magnification;// 技能倍率(万分比)
}
