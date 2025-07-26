using System.Collections;
using System.Collections.Generic;
using HFSM;
using UnityEngine;

public class BaseGameFSM : BaseFSM<EGameState>
{
    public BaseGameFSM(EGameState initialState) : base(false, initialState) { }
}
