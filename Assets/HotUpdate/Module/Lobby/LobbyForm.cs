using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LobbyForm : UIForm
{
    public override UIGroup DefultGroup => UIGroup.Middle;
    public override string Location => "Assets/GameAssets/UIPrefab/LobbyWnd.prefab";

    [Inject] public IMatchController MatchController;
    int _countTask;
    protected override void OnStart()
    {
        base.OnStart();

        this.Get<GameObject>("MatchInfoRoot").SetActive(false);
        this.Get<Button>("btnPvp").Subscribe(BtnPvpOnClick);
        this.Get<Button>("btnRank").Subscribe(BtnRankOnClick);
    }

    private void BtnRankOnClick()
    {
        SetMatchInfo(true);
        MatchController.Match();
    }

    private void BtnPvpOnClick()
    {
        SetMatchInfo(true);
        MatchController.Match();
    }

    void SetMatchInfo(bool show)
    {
        this.Get<GameObject>("MatchInfoRoot").SetActive(show);
        if (show)
        {
            this.Get<Text>("txtPredictTime").text = $"预计匹配时间 00:50";
            this.Get<Text>("txtCountTime").text = $"{00}:{00}";

            _countTask = GameEntry.Task.AddTask(taskInfo =>
            {
                int minute = (int)taskInfo.Times / 60;
                int second = (int)taskInfo.Times - 60 * minute;

                this.Get<Text>("txtCountTime").text = $"{minute:00}:{second:00}";
            }).Delay(TimeSpan.FromSeconds(1)).SetRepeatTimes(-1).Run();
        }
        else
        {
            GameEntry.Task.CancelTask(ref _countTask);
        }
    }
}
