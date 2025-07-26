using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class GameFSMRunner : MonoBehaviour
{
    [ShowInInspector] BaseGameFSM _fSM;

    void Awake()
    {
        _fSM = GameEntry.Procedure;
        GameObject.DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (_fSM.Active)
        {
            _fSM.Excute();
        }
    }
}
