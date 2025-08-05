using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFSM : BaseGameFSM
{
    public GameFSM(EGameState initialState = EGameState.Login) : base(initialState) { }

    protected override void OnInitialize()
    {
        base.OnInitialize();

        AddState(EGameState.Login, new LoginState());
        AddState(EGameState.LoadConfig, new LoadConfigState());
        AddState(EGameState.Lobby, new LobbyState());
        AddState(EGameState.LoadingGame, new LoadingGameState());
        AddState(EGameState.Combat, new CombatState());
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        GameEntry.UI.PushAsync<TipsForm>();
    }
}
