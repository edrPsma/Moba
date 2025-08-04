using System.Collections;
using System.Collections.Generic;
using Protocol;
using UnityEngine;

public interface IMatchController
{
    void Match();
}

[Controller]
public class MatchController : AbstarctController, IMatchController
{
    protected override void OnInitialize()
    {
        base.OnInitialize();
    }

    public void Match()
    {
        U2GS_Match msg = new U2GS_Match();

        GameEntry.Net.SendMsg(msg);
    }
}