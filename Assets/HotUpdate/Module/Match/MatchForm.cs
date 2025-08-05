using System;
using System.Collections;
using System.Collections.Generic;
using Observable;
using Protocol;
using UI;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;
using Zenject;

public class MatchForm : UIForm
{
    public override UIGroup DefultGroup => UIGroup.Middle;
    public override string Location => "Assets/GameAssets/UIPrefab/MatchWnd.prefab";

    [Inject] public IGameModel GameModel;
    [Inject] public IMatchController MatchController;
    int _taskID;
    const int Time = 15;

    protected override void OnStart()
    {
        base.OnStart();
        this.Get<Button>("btnConfirm").Subscribe(BtnComfirmOnClick);
        StartCountDown();

        GameModel.RoomDismiss.Register(OnRoomDismiss, true).Bind(Panel);
        GameModel.ComfirmDatas.Register(OnComfirmDatasChange, true).Bind(Panel);
    }

    protected override void OnClose()
    {
        base.OnClose();
        GameEntry.Task.CancelTask(ref _taskID);
    }

    void StartCountDown()
    {
        this.Get<Text>("txtTime").text = $"{Time}";
        _taskID = GameEntry.Task.AddTask(taskInfo =>
        {
            this.Get<Text>("txtTime").text = $"{Time - taskInfo.Times}";
        }).Delay(TimeSpan.FromSeconds(1)).SetRepeatTimes(Time - 1).Run();
    }

    private void BtnComfirmOnClick()
    {
        MatchController.Comfirm();
        this.Get<Button>("btnConfirm").interactable = false;
    }

    private void OnComfirmDatasChange(IReadOnlyListVariable<ComfirmData> variable)
    {
        GameObject[] leftArr = this.GetArray<GameObject>("left");
        GameObject[] rightArr = this.GetArray<GameObject>("right");

        SetIconState(leftArr, 0, variable);
        SetIconState(rightArr, variable.Count / 2, variable);

        int count = 0;
        for (int i = 0; i < variable.Count; i++)
        {
            if (variable[i].ComfirmDone)
            {
                count++;
            }
        }
        this.Get<Text>("txtConfirm").text = $"{count}/{variable.Count}";
    }

    void SetIconState(GameObject[] arr, int startIndex, IReadOnlyListVariable<ComfirmData> variable)
    {
        int num = variable.Count / 2;
        for (int i = 0; i < arr.Length; i++)
        {
            if (i < num)
            {
                arr[i].SetActive(true);
                arr[i].GetComponent<Image>().LoadSprite($"Assets/GameAssets/ResImages/MatchWnd/icon_{startIndex + i}.png");
                if (variable[startIndex + i].ComfirmDone)
                {
                    arr[i].transform.GetChild(0).SetActive(true);
                }
                else
                {
                    arr[i].transform.GetChild(0).SetActive(false);
                }
            }
            else
            {
                arr[i].SetActive(false);
            }
        }
    }

    private void OnRoomDismiss(bool dismiss)
    {
        if (dismiss)
        {
            GameEntry.UI.Pop<MatchForm>();
        }
    }
}
