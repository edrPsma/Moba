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