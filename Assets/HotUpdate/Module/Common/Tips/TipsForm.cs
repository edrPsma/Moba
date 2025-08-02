using System.Collections;
using System.Collections.Generic;
using Pool;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UI;

public class TipsForm : UIForm
{
    public override UIGroup DefultGroup => UIGroup.Popup;
    public override string Location => "Assets/GameAssets/UIPrefab/TipsWnd.prefab";

    Queue<string> _cacheShowList;
    HashSet<string> _cacheRemoveList;
    GameObjectPool<RectTransform> _pool;
    float _interval = 0.1f;
    float _moveTime = 0.5f;
    float _moveDelayTime = 0;
    bool _isInit;

    protected override void OnStart()
    {
        base.OnStart();
        _cacheShowList = new Queue<string>();
        _cacheRemoveList = new HashSet<string>();
        _pool = new GameObjectPool<RectTransform>(EPoolType.Scalable, 10, this.Get<RectTransform>("clone"), clone => clone.SetParent(this.Get<RectTransform>("recycleNode"), false), null);
        _pool.OnReleaseEvent += prefab => prefab.SetParent(this.Get<RectTransform>("recycleNode"), false);
        _pool.OnGetEvent += prefab =>
        {
            prefab.SetParent(GameObject, false);
            prefab.anchoredPosition = Vector2.zero;
        };

        this.Get<RectTransform>("clone").SetActive(false);
        Panel.StartCoroutine(DelayShow());
    }

    protected override void OnOpen()
    {
        base.OnOpen();
        if (!_isInit)
        {
            _isInit = true;
            // List<float> times = DataTable.GetItem<Global>("Prompt_Duration").ToValue<List<float>>();
            // _interval = times[0];
            // _interval = Mathf.Clamp(_interval, 0.1f, 10);
            // _moveTime = times[1];
            // _moveTime = Mathf.Clamp(_moveTime, 0.1f, 10);
            // _moveDelayTime = times[2];
            // _moveDelayTime = Mathf.Clamp(_moveDelayTime, 0, 10);
        }
    }

    void Show(string str)
    {
        if (!_cacheRemoveList.Contains(str))
        {
            _cacheShowList.Enqueue(str);
        }
    }

    IEnumerator DelayShow()
    {
        WaitForSeconds wait = new WaitForSeconds(_interval);
        while (true)
        {
            if (_cacheShowList.Count > 0)
            {
                RectTransform clone = _pool.SpawnByType();
                Text text = clone.Find<Text>("text");
                string str = _cacheShowList.Dequeue();
                text.text = str;
                _cacheRemoveList.Add(str);
                clone.DOAnchorPosY(130, _moveTime).SetDelay(_moveDelayTime).OnComplete(() =>
                {
                    _cacheRemoveList.Remove(str);
                    _pool.Release(clone);
                });
                yield return wait;
            }
            else
            {
                yield return null;
            }
        }
    }

    public static void ShowTips(string str)
    {
        GameEntry.UI.SearchForm<TipsForm>().Show(str);
    }
}
