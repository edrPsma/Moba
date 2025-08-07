using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using Zenject;

public interface IActorManager : ILogicController
{
    HeroActor SpawnHero(uint uid, int heroID, EActorLayer layer);

    HeroActor GetHero(uint uid);
}

[Controller]
public class ActorManager : AbstarctController, IActorManager
{
    [Inject] public IAssetSystem AssetSystem;
    [Inject] public IMoveSystem MoveSystem;
    [Inject] public IPlayerModel PlayerModel;
    Dictionary<int, LogicActor> _actorDic;
    Dictionary<uint, LogicActor> _heroDic;
    List<LogicActor> _actorList;
    int _actorID;

    protected override void OnInitialize()
    {
        base.OnInitialize();

        _actorDic = new Dictionary<int, LogicActor>();
        _actorList = new List<LogicActor>();
        _heroDic = new Dictionary<uint, LogicActor>();
    }

    public void LogicUpdate(FixInt deltaTime)
    {
        for (int i = 0; i < _actorList.Count; i++)
        {
            _actorList[i].LogicUpdate(deltaTime);
        }
    }

    public HeroActor SpawnHero(uint uid, int heroID, EActorLayer layer)
    {
        int actorID = GetActorID();

        DTHero table = DataTable.GetItem<DTHero>(heroID);

        GameObject clone = AssetSystem.Get<GameObject>($"Assets/GameAssets/Prefab/Chars/{table.Model}.prefab");
        HeroRenderingActor renderingActor = GameObject.Instantiate(clone).AddComponent<HeroRenderingActor>();
        HeroActor heroActor = new HeroActor(actorID, layer, renderingActor);
        GameScene scene = GameEntry.Scene.GetSceneState<GameScene>();
        heroActor.SetPosition(scene.GetSpawnPosition(layer));
        MoveSystem.AddUnit(heroActor.HitBox);

        // 设置相机跟随
        if (PlayerModel.UID == uid)
        {
            scene.CameraFollow.Target = renderingActor.transform;
        }

        _actorList.Add(heroActor);
        _actorDic.Add(actorID, heroActor);
        _heroDic.Add(uid, heroActor);
        return heroActor;
    }

    public HeroActor GetHero(uint uid)
    {
        if (_heroDic.ContainsKey(uid))
        {
            return _heroDic[uid] as HeroActor;
        }
        else
        {
            return null;
        }
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
