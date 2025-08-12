using System.Collections;
using System.Collections.Generic;
using Audio;
using Observable;
using Reflection;
using Scene;
using Task;
using UI;
using UnityEngine;
using YooAsset;

public static class GameEntry
{
    public static ResourcePackage Resource { get; private set; }
    public static TypeEventSource Event { get; private set; }
    public static BaseGameFSM Procedure { get; private set; }
    public static IUIManager UI { get; private set; }
    public static ISceneManager Scene { get; private set; }
    public static ITaskManager Task { get; private set; }
    public static IAudioManager Audio { get; private set; }
    public static INetManager Net { get; private set; }
    public static IReflectionManager Reflection { get; private set; }

    static bool _initialize;

    public static void Initialize(BaseGameFSM procedure, IUILoader uILoader)
    {
        if (_initialize)
        {
            Debug.LogError("GameEntry has already been initialized.");
            return;
        }

        _initialize = true;
        Reflection = ReflectionManager.Instance;
        Reflection.Init(new ReflectionHandler());
        Resource = YooAssets.GetPackage("DefaultPackage");
        Event = new TypeEventSource();
        UI = UIManager.Instance;
        UI.Initialize(uILoader);
        Scene = SceneManager.Instance;
        Task = TaskManager.Instance;
        Audio = AudioManager.Instance;
        Net = NetManager.Instance;
        Procedure = procedure;
        procedure.Initialize();
        procedure.Enter();
        new GameObject("[GameFSMRunner]").AddComponent<GameFSMRunner>();
    }
}
