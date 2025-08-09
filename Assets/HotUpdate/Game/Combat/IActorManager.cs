using System.Collections;
using System.Collections.Generic;
using System.Text;
using FixedPointNumber;
using UnityEngine;
using Zenject;

public interface IActorManager : ILogicController
{
    HeroActor SpawnHero(uint uid, int heroID, ECamp layer);

    HeroActor GetHero(uint uid);
}

[Controller]
public class ActorManager : AbstarctController, IActorManager
{
    [Inject] public ICombatSystem CombatSystem;
    [Inject] public IAssetSystem AssetSystem;
    [Inject] public IPhysicsSystem MoveSystem;
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

    public HeroActor SpawnHero(uint uid, int heroID, ECamp camp)
    {
        int actorID = GetActorID();

        DTHero table = DataTable.GetItem<DTHero>(heroID);

        GameObject clone = AssetSystem.Get<GameObject>($"Assets/GameAssets/Prefab/Chars/{table.Model}.prefab");
        HeroRenderingActor renderingActor = GameObject.Instantiate(clone).AddComponent<HeroRenderingActor>();
        HeroActor heroActor = new HeroActor(actorID, camp, camp == ECamp.Red ? ELayer.Layer1 : ELayer.Layer2, renderingActor);
        GameScene scene = GameEntry.Scene.GetSceneState<GameScene>();
        heroActor.SetPosition(scene.GetSpawnPosition(camp));
        MoveSystem.AddUnit(heroActor);

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
