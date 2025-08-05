using System;
using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using Observable;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class SelectHeroItem : MonoView
{
    [Inject] public IMatchController MatchController;
    [Inject] public IGameModel GameModel;
    Transform _select;
    int _heroID;

    public void Init()
    {
        _select = transform.Find("state");
        _select.SetActive(false);
        GetComponent<Button>().Subscribe(BtnOnSelect);

        GameEntry.Event.Register<EventSelectHero>(OnSelectHero).Bind(this);
    }

    public void Refresh(DTHero table)
    {
        _heroID = table.ID;
        transform.Find<Image>("imgIcon").LoadSprite($"Assets/GameAssets/ResImages/SelectWnd/{table.Icon}.png");
        transform.Find<Text>("txtName").text = table.Name;
        _select.SetActive(GameModel.SelectHeroID == _heroID);
    }

    private void BtnOnSelect()
    {
        MatchController.SelectHero(_heroID, false);
    }

    private void OnSelectHero(EventSelectHero e)
    {
        if (e.HeroID == _heroID)
        {
            _select.SetActive(true);
        }
        else
        {
            _select.SetActive(false);
        }
        GetComponent<Button>().interactable = !e.Comfirm;
    }
}
