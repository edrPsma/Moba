using System;
using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;
using Zenject;

public class CombatState : BaseState
{
    [Inject] public IAssetSystem AssetSystem;
    [Inject] public IActorManager ActorManager;
    [Inject] public IGameModel GameModel;
    [Inject] public ICombatSystem CombatSystem;

    public CombatState(bool hasExitTime = false) : base(hasExitTime) { }

    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入战斗流程");

        GameEntry.UI.HideGroup(UIGroup.Rear);
        GameEntry.UI.Pop<LoadingGameForm>();

        int len = GameModel.LoadInfo.Count;
        int half = len / 2;
        for (int i = 0; i < GameModel.LoadInfo.Count; i++)
        {
            int heroID = GameModel.LoadInfo[i].HeroID;
            uint uid = GameModel.LoadInfo[i].UId;
            if (i < half)
            {
                ActorManager.SpawnHero(uid, heroID, EActorLayer.Blue);
            }
            else
            {
                ActorManager.SpawnHero(uid, heroID, EActorLayer.Red);
            }
        }

        GameEntry.Task.AddTask(_ =>
        {
            CombatSystem.LogicUpdate(0.06667);
        }).Delay(TimeSpan.FromSeconds(0.06667)).SetRepeatTimes(-1).Run();
    }

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出战斗流程");

        GameEntry.UI.ShowGroup(UIGroup.Rear);
        AssetSystem.Dispose();
        GameEntry.UI.Pop<PlayForm>();
        GameEntry.Scene.UnloadChildScene<GameScene>();
    }
}
