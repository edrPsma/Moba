using System;
using UI;
using UnityEngine;

public interface IUIForm
{
    UIGroup DefultGroup { get; }
    string Location { get; }
    UIGroup Group { get; set; }
    EPanelState PanelState { get; set; }
    IUIPanel Panel { get; set; }
    GameObject GameObject { get; }
    object LoadUserData { get; set; }

    void Start();
    void Open();
    void Close();
    void Destroy();
}