using System.Collections;
using System.Collections.Generic;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class SelectHeroTuple : EnhancedScrollerCellView
{
    [SerializeField] SelectHeroItem _item1;
    [SerializeField] SelectHeroItem _item2;
    bool _initDone;

    public void Init()
    {
        if (_initDone) return;

        _initDone = true;
        _item1.Init();
        _item2.Init();
    }

    public void Refresh(DTHero[] arr, int index)
    {
        int startIndex = index * 2;
        _item1.Refresh(arr[startIndex]);
        if (arr.Length > startIndex + 1)
        {
            _item2.SetActive(true);
            _item2.Refresh(arr[startIndex + 1]);
        }
        else
        {
            _item2.SetActive(false);
        }
    }
}
