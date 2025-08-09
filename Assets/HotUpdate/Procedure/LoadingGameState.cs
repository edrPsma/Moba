using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using HFSM;
using UnityEngine;
using Zenject;

public class LoadingGameState : BaseState
{
    public LoadingGameState(bool hasExitTime = false) : base(hasExitTime) { }

    [Inject] public IMatchController MatchController;
    [Inject] public IGameModel GameModel;
    [Inject] public IAssetSystem AssetSystem;
    int _progress;
    GameScene _gameScene;
    int _assetCount;
    int _totalAssetCount;
    protected override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Procedure 进入加载游戏流程");

        GameEntry.UI.PushAsync<LoadingGameForm>();
        GameEntry.UI.PushAsync<PlayForm>();

        // 加载游戏资源 地图50 游戏资源50 
        LoadGameAsset();
        GameEntry.Scene.LoadScene<GameScene>();
        _gameScene = GameEntry.Scene.GetSceneState<GameScene>();
    }

    protected override void OnExcute()
    {
        base.OnExcute();

        int curProgress = _gameScene.Progress / 2 + (int)(_assetCount * 1f / _totalAssetCount * 50);

        if (_progress != curProgress)
        {
            _progress = _gameScene.Progress;
            Debug.Log(_progress);
            MatchController.LoadChange(_progress);
        }
    }

    void LoadGameAsset()
    {
        // 资源数 = 英雄数量 + 英雄资源配置 + 技能配置
        int heroCount = GameModel.LoadInfo.Count;
        int assetConfigCount = heroCount;
        int skillConfigCount = 0;

        for (int i = 0; i < heroCount; i++)
        {
            int heroID = GameModel.LoadInfo[i].HeroID;
            DTHero table = DataTable.GetItem<DTHero>(heroID);
            skillConfigCount += table.Skills.Length;
        }
        _totalAssetCount = heroCount + assetConfigCount + skillConfigCount;

        for (int i = 0; i < heroCount; i++)
        {
            int heroID = GameModel.LoadInfo[i].HeroID;
            DTHero table = DataTable.GetItem<DTHero>(heroID);
            AssetSystem.Preload<GameObject>($"Assets/GameAssets/Prefab/Chars/{table.Model}.prefab", AddAssetCount);
            AssetSystem.Preload<HeroAsset>($"Assets/GameAssets/So/HeroAsset/{table.Asset}.asset", AddAssetCount);
            for (int j = 0; j < table.Skills.Length; j++)
            {
                DTSkill dTSkill = DataTable.GetItem<DTSkill>(table.Skills[j]);
                AssetSystem.Preload<SkillConfig>($"Assets/GameAssets/So/Skill/{dTSkill.Config}.asset", AddAssetCount);
            }
        }
    }

    void AddAssetCount() => _assetCount++;

    protected override void OnExit()
    {
        base.OnExit();
        Debug.Log("Procedure 退出加载游戏流程");
    }
}
