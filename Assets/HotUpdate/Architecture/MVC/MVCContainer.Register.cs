
// 这个文件由代码生成器自动生成
using UnityEngine;
using Zenject;


public partial class MVCContainer
{
    static partial void BindController()
    {
		_container.BindInterfacesAndSelfTo<LoginController>().AsSingle();

		_container.BindInterfacesAndSelfTo<MatchController>().AsSingle();


    }

    static partial void BindModel()
    {
		_container.BindInterfacesAndSelfTo<PlayerModel>().AsSingle();

		_container.BindInterfacesAndSelfTo<GameModel>().AsSingle();


    }

}