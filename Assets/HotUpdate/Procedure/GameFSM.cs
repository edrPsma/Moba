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
        AddState(EGameState.SystemInit, new SystemInitState());
    }

    protected override void OnEnter()
    {
        base.OnEnter();
        GameEntry.UI.PushAsync<TipsForm>();
    }
}
