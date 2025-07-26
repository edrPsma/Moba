using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class FormA : UIForm
{
    public override UIGroup DefultGroup => UIGroup.Front;
    public override string Location => "Assets/GameAssets/UI/Test/PanelA.prefab";
}
