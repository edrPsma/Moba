using System;
using System.Collections;
using System.Collections.Generic;
using Observable;
using Protocol;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class LoadingGameForm : UIForm
{
    public override UIGroup DefultGroup => UIGroup.Front;
    public override string Location => "Assets/GameAssets/UIPrefab/LoadWnd.prefab";

    [Inject] public IGameModel GameModel;
    [Inject] public IPlayerModel PlayerModel;

    Text _progress;

    protected override void OnStart()
    {
        base.OnStart();

        GameEntry.UI.Pop<SelectHeroForm>();
        GameModel.LoadInfo.Register(OnProgressChange).Bind(Panel);
        Panel.StartCoroutine(ShowProgress());
    }


    private void OnProgressChange(IReadOnlyListVariable<LoadInfo> variable)
    {
        SetInfo("blue", 0, variable);
        SetInfo("red", variable.Count / 2, variable);
    }

    void SetInfo(string arrName, int startIndex, IReadOnlyListVariable<LoadInfo> variable)
    {
        GameObject[] arr = this.GetArray<GameObject>(arrName);

        int num = variable.Count / 2;
        for (int i = 0; i < arr.Length; i++)
        {
            if (i < num)
            {
                arr[i].SetActive(true);
                int index = startIndex + i;
                SetInfo(variable[index], arr[i]);
            }
            else
            {
                arr[i].SetActive(false);
            }
        }
    }

    void SetInfo(LoadInfo loadInfo, GameObject gameObject)
    {
        DTHero table = DataTable.GetItem<DTHero>(loadInfo.HeroID);
        gameObject.Find<Image>("imgHero").LoadSprite($"Assets/GameAssets/ResImages/Head/{table.Pic}.png");
        gameObject.Find<Text>("txtHeroName").text = table.Name;
        gameObject.Find<Text>("txtPlayerName").text = loadInfo.Name;

        if (PlayerModel.UID != loadInfo.UId)
        {
            gameObject.Find<Text>("txtProgress").text = $"{loadInfo.Progress}%";
        }
        else
        {
            if (_progress == null)
            {
                _progress = gameObject.Find<Text>("txtProgress");
            }
        }
    }

    IEnumerator ShowProgress()
    {
        while (true)
        {
            _progress.text = $"{GameEntry.Scene.GetSceneState<GameScene>().Progress}%";
            yield return null;
        }
    }
}
