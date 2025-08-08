using System.Collections;
using System.Collections.Generic;
using Observable;
using UnityEngine;

public interface IPlayerModel
{
    uint UID { get; set; }
    StringVariable Name { get; }
    List<int> HeroList { get; }
    GameConfig GameConfig { get; set; }
}

[Model]
public class PlayerModel : AbstractModel, IPlayerModel
{
    public uint UID { get; set; }
    public StringVariable Name { get; private set; }
    public List<int> HeroList { get; private set; }
    public GameConfig GameConfig { get; set; }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        HeroList = new List<int>();
        Name = new StringVariable();
    }
}