using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;
using YooAsset;
using Zenject;

public abstract class RenderingActor : MonoBehaviour
{
    [Inject] public IAssetSystem AssetSystem;
    [Inject] public ICombatSystem CombatSystem;
    public LogicActor LogicActor { get; private set; }

    public abstract Transform BodyTrans { get; protected set; }
    public abstract Transform HeadTrans { get; protected set; }

    const int PredicMaxCount = 15;
    [SerializeField] bool _predictPos = true;
    [SerializeField] bool _smoothPos = true;
    [SerializeField] bool _smoothRotate = true;

    [SerializeField] float _viewPosAcce = 10;
    [SerializeField] float _viewDirAccer = 10;
    [SerializeField] float _angleMultiplier = 8;
    Vector3 _targetPos;
    Vector3 _targetDir;
    int _predictCount;
    bool _isPosChange;

    Dictionary<string, int> _buffPrefabRefDic;// buff特效引用字典
    Dictionary<string, GameObject> _buffPrefabDic;



    public virtual void Initialize(LogicActor logicActor)
    {
        MVCContainer.Inject(this);
        LogicActor = logicActor;
        _buffPrefabRefDic = new Dictionary<string, int>();
        _buffPrefabDic = new Dictionary<string, GameObject>();
    }

    protected virtual void Update()
    {
        SyncMove();
        SyncDir();
    }

    public void UpdatePosition()
    {
        _predictCount = 0;
        _targetPos = LogicActor.Position.ToVector3();
        _isPosChange = true;
    }

    public void UpdateDirection()
    {
        _targetDir = LogicActor.Direction.ToVector3().normalized;
    }

    public void UpdatePositionForce()
    {
        _predictCount = 0;
        _targetPos = LogicActor.Position.ToVector3();
        transform.position = _targetPos;
    }

    public void UpdateDirectionForce()
    {
        _targetDir = LogicActor.Direction.ToVector3().normalized;
        if (_targetDir != Vector3.zero)
        {
            transform.forward = _targetDir;
        }
    }

    public virtual void PlayAnimation(string name)
    {

    }

    #region 移动平滑
    void SyncMove()
    {
        if (_predictPos)
        {
            if (!_isPosChange)
            {
                if (_predictCount > PredicMaxCount) return;

                Vector3 dir = LogicActor.Velocity.ToVector3().normalized;
                float deltaTime = Time.deltaTime;
                var predictPos = deltaTime * LogicActor.MoveSpeed.RawFloat * dir;
                _targetPos += predictPos;
                ++_predictCount;
            }
            else
            {
                if (transform.position != _targetPos)
                {
                    transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * _viewPosAcce);
                }
                else
                {
                    _isPosChange = false;
                }
            }

            if (_smoothPos)
            {
                transform.position = Vector3.Lerp(transform.position, _targetPos, Time.deltaTime * _viewPosAcce);
            }
            else
            {
                transform.position = _targetPos;
            }
        }
        else
        {
            transform.position = LogicActor.Position.ToVector3();
        }

        if (LogicActor.AIAgent.CanMove.Value == 0)
        {
            if (LogicActor.Velocity != FixIntVector3.zero)
            {
                PlayAnimation("run");
            }
            else
            {
                PlayAnimation("idle");
            }
        }

    }

    void SyncDir()
    {
        if (_smoothRotate)
        {
            float threshold = Time.deltaTime * _viewDirAccer;
            float angle = Vector3.Angle(transform.forward, _targetDir);
            float angleMult = (angle / 180) * _angleMultiplier * Time.deltaTime;

            if (_targetDir != Vector3.zero)
            {
                Vector3 interDir = Vector3.Lerp(transform.forward, _targetDir, threshold + angleMult);
                transform.forward = interDir;
            }
        }
        else
        {
            if (_targetDir != Vector3.zero)
            {
                transform.forward = _targetDir;
            }
        }
    }

    #endregion


    public void AddBuffPrefab(int buffId)
    {
        var data = DataTable.GetItem<DTSkill_buff>(buffId);
        if (string.IsNullOrEmpty(data.Prefab)) return;

        if (!_buffPrefabRefDic.ContainsKey(data.Prefab))
        {
            _buffPrefabRefDic.Add(data.Prefab, 1);

            if (!_buffPrefabDic.ContainsKey(data.Prefab))
            {
                GameObject prefab = GameObject.Instantiate(AssetSystem.GetHeroAsset<GameObject>($"Assets/GameAssets/Prefab/SkillPrefab/{data.Prefab}.prefab"));
                _buffPrefabDic.Add(data.Prefab, prefab);
                switch (data.Position)
                {
                    case 1: prefab.SetParent(transform); break;
                    case 2: prefab.SetParent(BodyTrans); break;
                    case 3: prefab.SetParent(HeadTrans); break;
                }
            }
        }
        else
        {
            _buffPrefabRefDic[data.Prefab]++;
        }
    }

    public void RemoveBuffPrefab(int buffId)
    {
        var data = DataTable.GetItem<DTSkill_buff>(buffId);
        if (string.IsNullOrEmpty(data.Prefab)) return;

        if (_buffPrefabRefDic.ContainsKey(data.Prefab))
        {
            _buffPrefabRefDic[data.Prefab]--;
            if (_buffPrefabRefDic[data.Prefab] <= 0)
            {
                _buffPrefabRefDic.Remove(data.Prefab);
                if (_buffPrefabDic.ContainsKey(data.Prefab))
                {
                    GameObject.Destroy(_buffPrefabDic[data.Prefab]);
                    _buffPrefabDic.Remove(data.Prefab);
                }
            }
        }
        else
        {
            Debug.LogError("Combat buff特效计数异常,BuffID: {buffId}");
        }
    }

    public void ClearAllBuffPrefab()
    {
        foreach (var item in _buffPrefabDic)
        {
            GameObject.Destroy(item.Value);
        }
        _buffPrefabDic.Clear();
        _buffPrefabRefDic.Clear();
    }


    public void PlayActionEffect(string location, Transform trans)
    {
        GameObject clone = AssetSystem.GetHeroAsset<GameObject>($"Assets/GameAssets/Prefab/SkillPrefab/{location}.prefab");
        AutoRecyclingEffects prefab = GameObject.Instantiate(clone).GetComponent<AutoRecyclingEffects>();
        prefab.SetParent(trans);
        prefab.PlayAndRecycle();
    }

    public Transform GetPartTrans(EPart part)
    {
        if (part == EPart.Bottom)
        {
            return transform;
        }
        else if (part == EPart.Body)
        {
            return BodyTrans;
        }
        else if (part == EPart.Head)
        {
            return HeadTrans;
        }

        return transform;
    }
}
