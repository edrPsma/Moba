using System;
using System.Collections;
using System.Collections.Generic;
using Template;
using UnityEngine;
using YooAsset;

namespace Scene
{
    public class SceneManager : MonoSingleton<ISceneManager, SceneManager>, ISceneManager
    {
        public ISceneState MainSceneState { get; private set; }
        Dictionary<Type, ISceneState> _sceneDic = new Dictionary<Type, ISceneState>();
        HashSet<ISceneState> _childScenes = new HashSet<ISceneState>();

        protected override void OnInit()
        {
            base.OnInit();
            gameObject.name = "[SceneManager]";
            GameObject.DontDestroyOnLoad(gameObject);
        }

        public void LoadScene<T>(bool suspendLoad = false) where T : ISceneState, new()
        {
            Type type = typeof(T);
            ISceneState state = GetHandler<T>(string.Empty);
            LoadScene(type, state, state.CurScenePath, suspendLoad);
        }

        public void LoadScene<T>(string location, bool suspendLoad = false) where T : ISceneState, new()
        {
            Type type = typeof(T);
            ISceneState state = GetHandler<T>(location);
            LoadScene(type, state, location, suspendLoad);
        }

        public void UnloadChildScene<T>() where T : ISceneState, new()
        {
            Type type = typeof(T);
            if (!_sceneDic.ContainsKey(type))
            {
                Debug.LogError($"Scene {type.Name} has not been loaded!");
                return;
            }

            ISceneState state = _sceneDic[type];
            if (state == MainSceneState)
            {
                Debug.LogError("A main scene must be kept!");
                return;
            }

            if (state.SceneState == ESceneLoadState.Unloading)
            {
                Debug.LogError($"Scene: {type.Name} is unloading");
            }
            else if (state.SceneState == ESceneLoadState.Unload)
            {
                Debug.LogError($"Scene: {type.Name} is unload");
            }
            else if (state.SceneState == ESceneLoadState.Loading)
            {
                UnloadSceneAsset(state);
                _childScenes.Remove(state);
            }
            else if (state.SceneState == ESceneLoadState.Loaded)
            {
                UnloadSceneAsset(state);
                _childScenes.Remove(state);
            }
        }

        public void SetActiveScene<T>()
        {
            Type type = typeof(T);
            if (!_sceneDic.ContainsKey(type))
            {
                Debug.LogError($"Scene {type.Name} has not been loaded!");
                return;
            }

            ISceneState state = _sceneDic[type];
            if (state.SceneState == ESceneLoadState.Loaded)
            {
                state.AssetHandle.UnSuspend();
            }
            else
            {
                Debug.LogError($"Scene {type.Name} has not been loaded!");
            }
        }

        public T GetSceneState<T>() where T : ISceneState, new()
        {
            Type type = typeof(T);

            return (T)_sceneDic.GetValue(type);
        }

        ISceneState GetHandler<T>(string location) where T : ISceneState, new()
        {
            Type type = typeof(T);
            if (!_sceneDic.ContainsKey(type))
            {
                _sceneDic.Add(type, new T());
            }

            ISceneState handler = _sceneDic[type];
            if (string.IsNullOrEmpty(location))
            {
                handler.CurScenePath = handler.DefultScenePath;
            }
            else
            {
                handler.CurScenePath = location;
            }

            return _sceneDic[type];
        }

        void LoadScene(Type type, ISceneState state, string location, bool suspendLoad = false)
        {
            if (!GameEntry.Resource.CheckLocationValid(location))
            {
                Debug.LogError("Scene Location is not Valid!");
                return;
            }

            if (state.SceneState == ESceneLoadState.Loading)
            {
                Debug.LogError($"Scene: {type.Name} is Loading");
            }
            else if (state.SceneState == ESceneLoadState.Loaded)
            {
                if (location == state.CurScenePath)
                {
                    Debug.LogError($"Scene: {type.Name} is Loaded");
                }
                else
                {
                    BeforeLoad(state);
                    LoadSceneAsset(location, state, suspendLoad);
                }
            }
            else if (state.SceneState == ESceneLoadState.Unloading)
            {
                Debug.LogError($"Scene: {type.Name} is Unloading");
            }
            else if (state.SceneState == ESceneLoadState.Unload)
            {
                BeforeLoad(state);
                LoadSceneAsset(location, state, suspendLoad);
            }
        }

        void LoadSceneAsset(string location, ISceneState state, bool suspendLoad)
        {
            var sceneMode = state.SceneMode;
            var physicsMode = UnityEngine.SceneManagement.LocalPhysicsMode.None;
            state.SceneState = ESceneLoadState.Loading;
            state.SceneSwitch();
            SceneHandle assetHandle = GameEntry.Resource.LoadSceneAsync(location, sceneMode, physicsMode, suspendLoad);
            state.AssetHandle = assetHandle;
            assetHandle.Completed += han =>
            {
                state.SceneState = ESceneLoadState.Loaded;
                state.SceneLoaded();
                GameEntry.Resource.UnloadUnusedAssetsAsync();
            };
        }

        void UnloadSceneAsset(ISceneState state)
        {
            state.SceneState = ESceneLoadState.Unloading;
            state.Exit();
            UnloadSceneOperation unloadSceneOperation = state.AssetHandle.UnloadAsync();
            unloadSceneOperation.Completed += op =>
            {
                state.AssetHandle = null;
                state.SceneState = ESceneLoadState.Unload;
            };
        }

        void BeforeLoad(ISceneState state)
        {
            if (state.SceneMode == UnityEngine.SceneManagement.LoadSceneMode.Single)
            {
                if (state != MainSceneState)
                {
                    foreach (var item in _childScenes)
                    {
                        UnloadSceneAsset(item);
                    }
                    _childScenes.Clear();
                    if (MainSceneState != null)
                    {
                        MainSceneState.SceneState = ESceneLoadState.Unload;
                        MainSceneState.AssetHandle = null;
                        MainSceneState.Exit();
                    }
                    MainSceneState = state;
                    state.Enter();
                }
            }
            else
            {
                state.Enter();
                _childScenes.Add(state);
            }
        }
    }
}