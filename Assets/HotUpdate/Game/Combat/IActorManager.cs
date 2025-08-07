using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using Zenject;

public interface IActorManager : ILogicController
{
    HeroActor SpawnHero(int heroID, EActorLayer layer);
}

[Controller]
public class ActorManager : AbstarctController, IActorManager
{
    [Inject] public IAssetSystem AssetSystem;
    [Inject] public IMoveSystem MoveSystem;
    Dictionary<int, LogicActor> _actorDic;
    List<LogicActor> _actorList;
    int _actorID;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _actorDic = new Dictionary<int, LogicActor>();
        _actorList = new List<LogicActor>();
    }

    public void LogicUpdate(FixInt deltaTime)
    {
        for (int i = 0; i < _actorList.Count; i++)
        {
            _actorList[i].LogicUpdate(deltaTime);
        }
    }

    public HeroActor SpawnHero(int heroID, EActorLayer layer)
    {
        int actorID = GetActorID();

        DTHero table = DataTable.GetItem<DTHero>(heroID);

        GameObject clone = AssetSystem.Get<GameObject>($"Assets/GameAssets/Prefab/Chars/{table.Model}.prefab");
        HeroRenderingActor renderingActor = GameObject.Instantiate(clone).AddComponent<HeroRenderingActor>();
        HeroActor heroActor = new HeroActor(actorID, layer, renderingActor);
        GameScene scene = GameEntry.Scene.GetSceneState<GameScene>();
        heroActor.SetPosition(scene.GetSpawnPosition(layer));
        MoveSystem.AddUnit(heroActor.HitBox);

        return heroActor;
    }

    int GetActorID()
    {
        while (_actorDic.ContainsKey(_actorID))
        {
            _actorID++;
        }

        return _actorID;
    }
}
