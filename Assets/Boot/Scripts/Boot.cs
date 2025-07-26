using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YooAsset;

public class Boot : MonoBehaviour
{
    public static StateMachine<EBootState> StateMachine { get; } = new StateMachine<EBootState>();
    public static EventManager Event { get; } = new EventManager();
    public static EPlayMode PlayMode => _instance._playMode;
    public static string ResServer => _instance._resServer;
    public static string PackageVersion { get; set; }

    static Boot _instance;
    [SerializeField] EPlayMode _playMode;
    [SerializeField] string _resServer;

    void Start()
    {
        _instance = this;
        Application.targetFrameRate = 60;
        Application.runInBackground = true;

        GameObject.Instantiate(Resources.Load<PatchWindow>("prefab/PatchWindow"));

        StateMachineInitialize();
        StateMachine.Run(EBootState.YooAssetInitialize);
    }

    void Update()
    {
        StateMachine.Update();
    }

    void OnDestroy()
    {
        StateMachine.Stop();
        Event.ClearEvents();
    }

    void StateMachineInitialize()
    {
        StateMachine.AddState(EBootState.YooAssetInitialize, new YooAssetInitializeState());
        StateMachine.AddState(EBootState.YooAssetInitPackage, new YooAssetInitPackageState());
        StateMachine.AddState(EBootState.CheckVersion, new CheckVersionState());
        StateMachine.AddState(EBootState.UpdateManifest, new UpdateManifestState());
        StateMachine.AddState(EBootState.CreateDownloader, new CreateDownloaderState());
        StateMachine.AddState(EBootState.Download, new DownloadState());
        StateMachine.AddState(EBootState.ClearCache, new ClearCacheState());
        StateMachine.AddState(EBootState.LoadAssembly, new LoadAssemblyState());
        StateMachine.AddState(EBootState.EnterGame, new EnterGameState());
    }
}

public enum EBootState
{
    YooAssetInitialize,
    YooAssetInitPackage,
    CheckVersion,
    UpdateManifest,
    CreateDownloader,
    Download,
    ClearCache,
    LoadAssembly,
    EnterGame,
}