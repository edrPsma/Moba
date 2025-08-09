using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public readonly struct EventSelectHero
{
    public int HeroID { get; }
    public bool Comfirm { get; }

    public EventSelectHero(int heroID, bool comfirm)
    {
        HeroID = heroID;
        Comfirm = comfirm;
    }
}

public readonly struct EventShowSkillIndicator
{
    public ESkillReleaseType SkillReleaseType { get; }
    public Vector2 Vector { get; }
    public float Area { get; }
    public float Area2 { get; }

    public EventShowSkillIndicator(ESkillReleaseType type, Vector2 vector, float area, float area2)
    {
        SkillReleaseType = type;
        Vector = vector;
        Area = area;
        Area2 = area2;
    }
}