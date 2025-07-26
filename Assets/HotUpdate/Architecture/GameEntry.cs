using System.Collections;
using System.Collections.Generic;
using Observable;
using Scene;
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

    static bool _initialize;

    public static void Initialize(BaseGameFSM procedure, IUILoader uILoader)
    {
        if (_initialize)
        {
            Debug.LogError("GameEntry has already been initialized.");
            return;
        }

        _initialize = true;
        Resource = YooAssets.GetPackage("DefaultPackage");
        Event = new TypeEventSource();
        UI = UIManager.Instance;
        UI.Initialize(uILoader);
        Scene = SceneManager.Instance;
        Procedure = procedure;
        procedure.Initialize();
        procedure.Enter();
        new GameObject("[GameFSMRunner]").AddComponent<GameFSMRunner>();
    }
}
