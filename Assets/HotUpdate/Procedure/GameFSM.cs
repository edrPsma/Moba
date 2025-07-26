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
    }
}
