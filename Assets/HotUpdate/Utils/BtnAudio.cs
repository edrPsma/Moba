using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class BtnAudio : MonoBehaviour
{
    [SerializeField] string _name;

    void Start()
    {
        GetComponent<Button>().Subscribe(BtnOnClick);
    }

    private void BtnOnClick()
    {
        GameEntry.Audio.PlayEffect($"Assets/GameAssets/Audio/{_name}.mp3");
    }
}
