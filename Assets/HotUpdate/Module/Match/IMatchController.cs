using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Protocol;
using UnityEngine;
using Zenject;

public interface IMatchController
{
    void Recover();

    void Match();

    void Comfirm();

    void SelectHero(int heroID, bool comfirm);

    void LoadChange(int progress);
}

[Controller]
public class MatchController : AbstarctController, IMatchController
{
    [Inject] public IGameModel GameModel;

    protected override void OnInitialize()
    {
        base.OnInitialize();
        GameEntry.Net.Register<GS2U_Comfirm>(OnComfirm);
        GameEntry.Net.Register<GS2U_Load>(OnLoad);
        GameEntry.Net.Register<GS2U_StartLoad>(OnStartLoad);
        GameEntry.Net.Register<GS2U_Battle>(OnStartBattle);
    }

    public void Recover()
    {
        U2GS_TryRecover msg = new U2GS_TryRecover();

        GameEntry.Net.SendMsg(msg);
    }

    public void Match()
    {
        U2GS_Match msg = new U2GS_Match();

        GameEntry.Net.SendMsg(msg);
    }

    public void Comfirm()
    {
        U2GS_Comfirm msg = new U2GS_Comfirm
        {
            RoomID = GameModel.RoomID
        };

        GameEntry.Net.SendMsg(msg);
    }

    public void SelectHero(int heroID, bool comfirm)
    {
        if (comfirm)
        {
            U2GS_SelectHero msg = new U2GS_SelectHero
            {
                RoomID = GameModel.RoomID,
                HeroID = heroID,
            };

            GameEntry.Net.SendMsg(msg);
        }
        GameModel.SelectHeroID = heroID;
        GameEntry.Event.Trigger(new EventSelectHero(heroID, comfirm));
    }

    private void OnComfirm(GS2U_Comfirm comfirm)
    {
        GameModel.RoomID = comfirm.RoomID;

        GameModel.ComfirmDatas.Modifly(list =>
        {
            list.Clear();
            list.AddRange(comfirm.ComfirmArr);
        });
        GameModel.RoomDismiss.Value = comfirm.Dismiss;

        if (comfirm.Dismiss)
        {
            GameEntry.UI.Pop<MatchForm>();
        }
        else
        {
            if (comfirm.ComfirmArr.Any(item => !item.ComfirmDone))
            {
                GameEntry.UI.PushAsync<MatchForm>();
            }
            else
            {
                GameEntry.UI.PushAsync<SelectHeroForm>();
            }
        }
    }

    private void OnLoad(GS2U_Load msg)
    {
        GameModel.LoadInfo.Modifly(list =>
        {
            list.Clear();
            list.AddRange(msg.LoadInfo);
        });
    }

    private void OnStartLoad(GS2U_StartLoad msg)
    {
        GameModel.RoomID = msg.RoomID;
        GameModel.LoadInfo.Modifly(list =>
        {
            list.AddRange(msg.LoadInfo);
        });
        GameEntry.Procedure.TransitionImmediately(EGameState.LoadingGame);
    }

    public void LoadChange(int progress)
    {
        U2GS_Load msg = new U2GS_Load
        {
            RoomID = GameModel.RoomID,
            Progress = progress,
        };

        GameEntry.Net.SendMsg(msg);
    }

    private void OnStartBattle(GS2U_Battle battle)
    {
        GameEntry.Procedure.TransitionImmediately(EGameState.Combat);
    }
}