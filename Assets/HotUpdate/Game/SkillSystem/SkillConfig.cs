using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "SkillConfig")]
public class SkillConfig : ScriptableObject
{
    public string AnimationName;
    public int[] Times = new int[] { 0 };
    public ESkillRuleType RuleType;
    public int[] RuleValues;

    public int SelectArea;
    public EDamageAreaType DamageAreaType;
    public int DamageArea;

    public int Duration;
    public int Delay;

    public int Interval;
    public string ActionPrefabPath;
    public string SkillPrefabPath;
    public Vector3 SkillPrefabModify = Vector3.forward;

    public string SkillBoomPrefabPath;
    public float SkillBoomPrefabScale = 1;

}
