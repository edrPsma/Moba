using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scene
{
    public interface ISceneManager
    {
        void LoadScene<T>(bool suspendLoad = false) where T : ISceneState, new();

        void LoadScene<T>(string location, bool suspendLoad = false) where T : ISceneState, new();

        void UnloadChildScene<T>() where T : ISceneState, new();

        void SetActiveScene<T>();

        T GetSceneState<T>() where T : ISceneState, new();
    }
}