///////////////////////////////////
///该代码由工具自动生成，请勿修改////
///////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[System.Serializable]
public class DTTest : IDataTable
{
	string IDataTable.Location => "Test";
	[ShowInInspector] public readonly int ID;// ID
	[ShowInInspector] public readonly string Name;// 名字
	[ShowInInspector] public readonly int[] Data;// 数据
	[ShowInInspector] public readonly int[][] Data2;// 数据2
	[ShowInInspector] public readonly int Num;// 编号
}
