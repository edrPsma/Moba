using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Observable;
using UI;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;
using Zenject;

public class SelectHeroForm : UIForm, IEnhancedScrollerDelegate
{
    public override UIGroup DefultGroup => UIGroup.Middle;
    public override string Location => "Assets/GameAssets/UIPrefab/SelectWnd.prefab";

    const int SelectHeroTime = 30;
    [Inject] public IMatchController MatchController;
    [Inject] public IGameModel GameModel;
    DTHero[] _datas;
    GameObject _heroPrefab;
    AssetHandle _assetHandle;
    int _taskID;

    protected override void OnStart()
    {
        base.OnStart();
        GameEntry.UI.Pop<MatchForm>();
        GameEntry.UI.SearchForm<LobbyForm>()?.SetMatchInfo(false);

        this.Get<Button>("btnSure").Subscribe(BtnSureOnClick);
        GameEntry.Event.Register<EventSelectHero>(OnSelectHero).Bind(Panel);
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        _datas = DataTable.GetArray<DTHero>();
        MatchController.SelectHero(101, false);
        this.Get<EnhancedScroller>("Scroll View").Delegate = this;
        RefreshScrollView();
        CountDown();
    }

    protected override void OnClose()
    {
        base.OnClose();
        GameEntry.Task.CancelTask(ref _taskID);
        _assetHandle?.Release();
    }

    private void OnSelectHero(EventSelectHero e)
    {
        _assetHandle?.Release();
        GameObject.Destroy(_heroPrefab);

        DTHero table = DataTable.GetItem<DTHero>(e.HeroID);

        SetSkillIncos(table);
        _assetHandle = GameEntry.Resource.LoadAssetAsync<GameObject>($"Assets/GameAssets/Prefab/Chars/{table.Model}.prefab");
        _assetHandle.Completed += handle =>
        {
            _heroPrefab = GameObject.Instantiate(handle.AssetObject as GameObject);
            this.Get<ModelViewer>("ModelViewer").transform.localScale = Vector3.one;
            this.Get<ModelViewer>("ModelViewer").ShowModel(_heroPrefab);
            this.Get<ModelViewer>("ModelViewer").transform.localScale = Vector3.one * 5;
        };

        if (e.Comfirm)
        {
            this.Get<Button>("btnSure").interactable = false;
        }
    }

    private void SetSkillIncos(DTHero table)
    {
        Image[] images = this.GetArray<Image>("skills");
        for (int i = 0; i < images.Length; i++)
        {
            DTSkill dTSkill = DataTable.GetItem<DTSkill>(table.ShowSkills[i]);
            images[i].LoadSprite($"Assets/GameAssets/ResImages/PlayWnd/{dTSkill.Icon}.png");
        }
    }

    private void CountDown()
    {
        this.Get<Text>("txtConfirm").text = $"00:{SelectHeroTime}";
        _taskID = GameEntry.Task.AddTask(taskInfo =>
        {
            this.Get<Text>("txtConfirm").text = $"00:{SelectHeroTime - taskInfo.Times:00}";
        }).Delay(TimeSpan.FromSeconds(1)).SetRepeatTimes(SelectHeroTime - 1).OnComplete(BtnSureOnClick).Run();
    }

    private void BtnSureOnClick()
    {
        MatchController.SelectHero(GameModel.SelectHeroID, true);
    }

    private void RefreshScrollView()
    {
        this.Get<EnhancedScroller>("Scroll View").ReloadData();
        this.Get<SelectHeroTuple>("HeroTuple").SetActive(false);
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return Mathf.FloorToInt(_datas.Length / 2f);
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 200f;
    }

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        SelectHeroTuple cellView = scroller.GetCellView(this.Get<SelectHeroTuple>("HeroTuple")) as SelectHeroTuple;

        cellView.Init();
        cellView.Refresh(_datas, cellIndex);
        return cellView;
    }
}
