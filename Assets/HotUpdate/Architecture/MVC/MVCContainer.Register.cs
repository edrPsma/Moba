
// 这个文件由代码生成器自动生成
using UnityEngine;
using Zenject;


public partial class MVCContainer
{
    static partial void BindController()
    {
		_container.Bind(typeof(ILoginController)).To<LoginController>().AsSingle();

    }

    static partial void BindModel()
    {

    }

}