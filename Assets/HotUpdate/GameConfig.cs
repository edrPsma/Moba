using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig")]
public class GameConfig : ScriptableObject
{
    public string RemoteIP;
    public int Port;
    public bool TestMode;
}
