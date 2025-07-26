using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Combat/IgnoreConfig", fileName = "IgnoreConfig", order = 20)]
public class IgnoreConfig : ScriptableObject
{
    public List<string> CSVIgnoreList = new List<string>();

    public List<string> ScriptIgnoreList = new List<string>();
}
