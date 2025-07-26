using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Observable
{
    /// <summary>
    /// 注销事件触发器
    /// </summary>
    public class UnRegisterTrigger : MonoBehaviour
    {
        HashSet<IUnRegister> _unregisters = new HashSet<IUnRegister>();

        /// <summary>
        /// 添加事件注销器
        /// </summary>
        /// <param name="unRegister">事件注销器</param>
        public void AddUnRegister(IUnRegister unRegister)
        {
            _unregisters.Add(unRegister);
        }

        private void OnDestroy()
        {
            foreach (var item in _unregisters)
            {
                item.UnRegister();
            }

            _unregisters.Clear();
            _unregisters = null;
        }
    }

    /// <summary>
    /// 事件注销器扩展类
    /// </summary>
    public static class UnRegisterExtra
    {
        /// <summary>
        /// 在游戏物体销毁时注销事件
        /// </summary>
        /// <param name="unRegister">事件注销器</param>
        /// <param name="gameObject">游戏物体</param>
        public static IUnRegister Bind(this IUnRegister unRegister, GameObject gameObject)
        {
            var trigger = gameObject.GetComponent<UnRegisterTrigger>();

            if (!trigger)
            {
                trigger = gameObject.AddComponent<UnRegisterTrigger>();
            }

            trigger.AddUnRegister(unRegister);

            return unRegister;
        }

        /// <summary>
        /// 在游戏物体销毁时注销事件
        /// </summary>
        /// <param name="unRegister">事件注销器</param>
        /// <param name="component">游戏物体</param>
        public static IUnRegister Bind(this IUnRegister unRegister, Component component)
        {
            return Bind(unRegister, component.gameObject);
        }

        /// <summary>
        /// 在游戏物体销毁时注销事件
        /// </summary>
        /// <param name="unRegister">事件注销器</param>
        /// <param name="mono">游戏物体</param>
        public static IUnRegister Bind(this IUnRegister unRegister, MonoBehaviour mono)
        {
            return Bind(unRegister, mono.gameObject);
        }
    }
}