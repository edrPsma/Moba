using System.Collections;
using System.Collections.Generic;
using Scene;
using UnityEngine;
using UnityEngine.SceneManagement;
using YooAsset;

public abstract class BaseSceneState : ISceneState
{
    public abstract string DefultScenePath { get; }
    public abstract LoadSceneMode SceneMode { get; }
    public string CurScenePath { get; set; }
    SceneHandle ISceneState.AssetHandle { get; set; }
    ESceneLoadState ISceneState.SceneState { get; set; }

    void ISceneState.Enter()
    {
        OnEnter();
    }

    void ISceneState.Exit()
    {
        OnExit();
    }

    void ISceneState.SceneLoaded()
    {
        OnSceneLoaded();
    }

    public void UnSuspend()
    {
        this.As<ISceneState>().AssetHandle?.UnSuspend();
    }

    void ISceneState.SceneSwitch()
    {
        OnSwitch();
    }

    protected virtual void OnEnter() { }
    protected virtual void OnSceneLoaded() { }
    protected virtual void OnExit() { }
    protected virtual void OnSwitch() { }

    protected GameObject FindRootObject(string name)
    {
        GameObject[] gameObjects = this.As<ISceneState>().AssetHandle.SceneObject.GetRootGameObjects();
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (gameObjects[i].name == name)
            {
                return gameObjects[i];
            }
        }

        return null;
    }

    protected GameObject[] GetRootGameObjects()
    {
        return this.As<ISceneState>().AssetHandle.SceneObject.GetRootGameObjects();
    }
}
