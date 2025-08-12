using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillConfig", menuName = "SkillConfig")]
public class SkillConfig : ScriptableObject
{
    public string AnimationName;
    public int[] Times = new int[] { 0 };
    public ESkillRuleType RuleType;
    public int[] RuleValues;

    public ETargetType TargetType;
    public int SelectArea;
    public ESkillReleaseType ReleaseType;
    [ShowIf(nameof(ShowBulletPlace))] public EBulletPlace BulletPlace;
    public EDamageAreaType DamageAreaType;
    public int[] DamageArea;

    public EDestoryType DestoryType;
    public int Duration;
    public int Delay;

    public int Interval;
    public string ActionPrefabPath;
    public string SkillPrefabPath;
    public Vector3 SkillPrefabModify = Vector3.forward;

    public string SkillBoomPrefabPath;
    public float SkillBoomPrefabScale = 1;

    bool ShowBulletPlace()
    {
        if (ReleaseType == ESkillReleaseType.TargetUnit)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
