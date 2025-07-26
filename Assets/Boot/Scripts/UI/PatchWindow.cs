using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PatchWindow : MonoBehaviour
{
    [SerializeField] Text _progressTxt;
    [SerializeField] Slider _progress;
    [SerializeField] GameObject _messageBox;
    [SerializeField] Text _tipsTxt;
    [SerializeField] Button _btnOk;

    Action _callBack;

    void Awake()
    {
        Boot.Event.RegisterEvent<EventInitialize>(OnInitialize).Bind(gameObject);
        Boot.Event.RegisterEvent<EventCheckVersion>(OnCheckVersion).Bind(gameObject);
        Boot.Event.RegisterEvent<EventDownloadProgress>(OnDownloadProgress).Bind(gameObject);
        Boot.Event.RegisterEvent<EventEnterGame>(OnEnterGame).Bind(gameObject);
        Boot.Event.RegisterEvent<EventShowTips>(OnShowTips).Bind(gameObject);
        _btnOk.onClick.AddListener(OnOkClick);
    }

    private void OnInitialize(EventInitialize e)
    {
        _progress.value = 0;
        _progressTxt.text = "正在初始化";
    }

    private void OnCheckVersion(EventCheckVersion e)
    {
        _progress.value = 0;
        _progressTxt.text = "正在检查版本更新";
    }

    private void OnDownloadProgress(EventDownloadProgress msg)
    {
        _progress.value = (float)msg.CurrentDownloadCount / msg.TotalDownloadCount;
        string currentSizeMB = (msg.CurrentDownloadSizeBytes / 1048576f).ToString("f1");
        string totalSizeMB = (msg.TotalDownloadSizeBytes / 1048576f).ToString("f1");
        _progressTxt.text = $"{msg.CurrentDownloadCount}/{msg.TotalDownloadCount} {currentSizeMB}MB/{totalSizeMB}MB";
    }

    private void OnEnterGame(EventEnterGame game)
    {
        GameObject.Destroy(gameObject);
    }

    private void OnShowTips(EventShowTips tips)
    {
        _messageBox.SetActive(true);
        _callBack = tips.CallBack;
        _tipsTxt.text = tips.Content;
    }

    private void OnOkClick()
    {
        _callBack?.Invoke();
        _messageBox.SetActive(false);
    }
}
