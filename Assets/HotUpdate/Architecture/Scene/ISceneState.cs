using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

namespace Scene
{
    public interface ISceneState
    {
        string DefultScenePath { get; }
        string CurScenePath { get; set; }
        SceneHandle AssetHandle { get; set; }
        ESceneLoadState SceneState { get; set; }
        LoadSceneMode SceneMode { get; }

        void Enter();

        void SceneSwitch();

        void SceneLoaded();

        void UnSuspend();

        void Exit();
    }

    public enum ESceneLoadState
    {
        Unload = 0,
        Unloading = 1,
        Loading = 2,
        Loaded = 3,
    }
}