using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using Observable;
using UI;
using UnityEngine;
using Zenject;

public class PlayForm : UIForm
{
    public override UIGroup DefultGroup => UIGroup.FrontMost;
    public override string Location => "Assets/GameAssets/UIPrefab/PlayWnd.prefab";

    [Inject] public IOperateSystem OperateSystem;
    [Inject] public IGameModel GameModel;
    [Inject] public IPlayerModel PlayerModel;

    FloatingJoystick _joystick;
    Vector3 _lastDir;

    protected override void OnStart()
    {
        base.OnStart();
        _joystick = this.Get<FloatingJoystick>("Joystick");
        OperateSystem.OnLogicUpdate += OnLogicUpdate;

        GameEntry.Event.Register<EventStartCombat>(OnCombatStart).Bind(Panel);
    }

    private void OnCombatStart(EventStartCombat combat)
    {
        InitSkillItem();
    }

    void InitSkillItem()
    {
        int heroID = 101;
        foreach (var item in GameModel.LoadInfo)
        {
            if (PlayerModel.UID == item.UId)
            {
                heroID = item.HeroID;
            }
        }
        DTHero table = DataTable.GetItem<DTHero>(heroID);
        SkillReleaseItem[] skillReleaseItems = this.GetArray<SkillReleaseItem>("skillArr");
        for (int i = 0; i < skillReleaseItems.Length; i++)
        {
            skillReleaseItems[i].Refresh(table.ShowSkills[i]);
        }
    }

    private void OnLogicUpdate(FixInt deltaTime)
    {

        Vector3 dir = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical).normalized;

        if (dir == Vector3.zero)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");
            dir = new Vector3(x, 0, z).normalized;
        }

        if (_lastDir == dir && dir == Vector3.zero)
        {
            return;
        }

        Quaternion rotation = Quaternion.Euler(0, 45, 0);
        Vector3 rotated = rotation * dir;
        OperateSystem.SendMoveOperate(new FixIntVector3(rotated));
        _lastDir = dir;
    }
}
