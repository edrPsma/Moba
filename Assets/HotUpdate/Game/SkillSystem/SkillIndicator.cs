using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIndicator : MonoBehaviour
{
    [SerializeField] Transform _area;
    [SerializeField] Transform _area2;
    [SerializeField] Transform _arrow;
    [SerializeField] Transform _sector;

    void Start()
    {
        GameEntry.Event.Register<EventShowSkillIndicator>(OnShowIndicator);
    }

    private void OnShowIndicator(EventShowSkillIndicator e)
    {
        if (e.SkillReleaseType == ESkillReleaseType.NoTarget)
        {
            _area.SetActive(false);
            _area2.SetActive(false);
            _arrow.SetActive(false);
            _sector.SetActive(false);
        }
        else if (e.SkillReleaseType == ESkillReleaseType.TargetUnit)
        {
            _area.SetActive(true);
            _area2.SetActive(false);
            _arrow.SetActive(false);
            _sector.SetActive(false);
            _area.transform.localScale = Vector3.one * e.Area;
        }
        else if (e.SkillReleaseType == ESkillReleaseType.TargetPoint)
        {
            _area.SetActive(true);
            _area2.SetActive(true);
            _arrow.SetActive(false);
            _sector.SetActive(false);
            _area.transform.localScale = Vector3.one * e.Area;
            _area2.transform.localScale = Vector3.one * e.Area2;

            Vector3 dir = new Vector3(e.Vector.x, 0, e.Vector.y);
            Quaternion rotation = Quaternion.Euler(0, 45, 0);
            Vector3 rotated = rotation * dir;

            Vector3 pos = rotated * e.Area * 0.5f;
            _area2.transform.localPosition = pos;
        }
        else if (e.SkillReleaseType == ESkillReleaseType.VectorSkill)
        {
            _area.SetActive(false);
            _area2.SetActive(false);
            _arrow.SetActive(true);
            _sector.SetActive(false);
            Vector3 dir = new Vector3(e.Vector.x, 0, e.Vector.y);
            Quaternion rotation = Quaternion.Euler(0, 45, 0);
            Vector3 rotated = rotation * dir;
            _arrow.transform.localScale = Vector3.one * e.Area;
            _arrow.transform.forward = -rotated;
        }
    }
}
