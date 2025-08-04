using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPlayerModel
{
    public uint UID { get; set; }
    public List<int> HeroList { get; }
}

[Model]
public class PlayerModel : AbstractModel, IPlayerModel
{
    public uint UID { get; set; }
    public List<int> HeroList { get; private set; }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        HeroList = new List<int>();
    }
}