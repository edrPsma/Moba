using System.Collections;
using System.Collections.Generic;
using Observable;
using Protocol;
using UnityEngine;

public interface IGameModel
{
    int RoomID { get; set; }
    BoolVariable RoomDismiss { get; }
    ListVariable<ComfirmData> ComfirmDatas { get; }
    int SelectHeroID { get; set; }
    ListVariable<LoadInfo> LoadInfo { get; }
}

[Model]
public class GameModel : AbstractModel, IGameModel
{
    public int RoomID { get; set; }
    public BoolVariable RoomDismiss { get; private set; }
    public ListVariable<ComfirmData> ComfirmDatas { get; private set; }
    public int SelectHeroID { get; set; }
    public ListVariable<LoadInfo> LoadInfo { get; private set; }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        RoomDismiss = new BoolVariable();
        ComfirmDatas = new ListVariable<ComfirmData>();
        LoadInfo = new ListVariable<LoadInfo>();
    }
}