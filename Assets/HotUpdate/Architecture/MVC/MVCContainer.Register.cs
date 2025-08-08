
// 这个文件由代码生成器自动生成
using UnityEngine;
using Zenject;


public partial class MVCContainer
{
    static partial void BindController()
    {
		_container.BindInterfacesAndSelfTo<AssetSystem>().AsSingle();

		_container.BindInterfacesAndSelfTo<ActorManager>().AsSingle();

		_container.BindInterfacesAndSelfTo<CombatSystem>().AsSingle();

		_container.BindInterfacesAndSelfTo<MoveSystem>().AsSingle();

		_container.BindInterfacesAndSelfTo<OperateSystem>().AsSingle();

		_container.BindInterfacesAndSelfTo<LoginController>().AsSingle();

		_container.BindInterfacesAndSelfTo<MatchController>().AsSingle();


    }

    static partial void BindModel()
    {
		_container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle();

		_container.BindInterfacesAndSelfTo<GameModel>().AsSingle();


    }

}