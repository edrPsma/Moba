using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class PlayForm : UIForm
{
    public override UIGroup DefultGroup => UIGroup.FrontMost;
    public override string Location => "Assets/GameAssets/UIPrefab/PlayWnd.prefab";

    protected override void OnStart()
    {
        base.OnStart();
    }
}
