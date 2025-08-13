using System;
using System.Collections;
using System.Collections.Generic;
using FixedPointNumber;
using UnityEngine;

public class SkillIndicator : MonoBehaviour
{
    [SerializeField] Transform _area;
    [SerializeField] Transform _area2;
    [SerializeField] Transform _arrow;
    [SerializeField] Transform _sector;
    [SerializeField] LineRenderer _lineRenderer;

    void Start()
    {
        GameEntry.Event.Register<EventShowSkillIndicator>(OnShowIndicator);
        _area.SetActive(false);
        _area2.SetActive(false);
        _arrow.SetActive(false);
        _sector.SetActive(false);
    }

    private void OnShowIndicator(EventShowSkillIndicator e)
    {
        if (e.SkillReleaseType == ESkillReleaseType.NoTarget)
        {
            _area.SetActive(false);
            _area2.SetActive(false);
            _arrow.SetActive(false);
            _sector.SetActive(false);
            _lineRenderer.SetActive(false);
        }
        else if (e.SkillReleaseType == ESkillReleaseType.TargetUnit)
        {
            _area.SetActive(true);
            _area2.SetActive(false);
            _arrow.SetActive(false);
            _sector.SetActive(false);
            _lineRenderer.SetActive(true);
            _area.transform.localScale = Vector3.one * e.Area;

            if (e.Target != null)
            {
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, e.Target.Rendering.transform.position);
            }
            else
            {
                Vector3 dir = new Vector3(e.Vector.x, 0, e.Vector.y);
                Quaternion rotation = Quaternion.Euler(0, 45, 0);
                Vector3 rotated = rotation * dir;

                Vector3 pos = rotated * e.Area;
                _lineRenderer.SetPosition(0, transform.position);
                _lineRenderer.SetPosition(1, pos + transform.position);
            }
        }
        else if (e.SkillReleaseType == ESkillReleaseType.TargetPoint)
        {
            _area.SetActive(true);
            _area2.SetActive(true);
            _arrow.SetActive(false);
            _sector.SetActive(false);
            _lineRenderer.SetActive(false);
            _area.transform.localScale = Vector3.one * e.Area;
            _area2.transform.localScale = Vector3.one * e.Area2;

            Vector3 dir = new Vector3(e.Vector.x, 0, e.Vector.y);
            Quaternion rotation = Quaternion.Euler(0, 45, 0);
            Vector3 rotated = rotation * dir;

            Vector3 pos = rotated * e.Area;
            _area2.transform.localPosition = pos;
        }
        else if (e.SkillReleaseType == ESkillReleaseType.VectorSkill)
        {
            _area.SetActive(false);
            _area2.SetActive(false);
            _arrow.SetActive(true);
            _sector.SetActive(false);
            _lineRenderer.SetActive(false);
            Vector3 dir = new Vector3(e.Vector.x, 0, e.Vector.y);
            Quaternion rotation = Quaternion.Euler(0, 45, 0);
            Vector3 rotated = rotation * dir;
            _arrow.transform.localScale = Vector3.one * e.Area;
            _arrow.transform.forward = -rotated;
        }
        else if (e.SkillReleaseType == ESkillReleaseType.SectorVectorSkill)
        {
            _area.SetActive(false);
            _area2.SetActive(false);
            _arrow.SetActive(false);
            _sector.SetActive(true);
            _lineRenderer.SetActive(false);
            Vector3 dir = new Vector3(e.Vector.x, 0, e.Vector.y);
            Quaternion rotation = Quaternion.Euler(0, 45, 0);
            Vector3 rotated = rotation * dir;
            _sector.transform.localScale = Vector3.one * e.Area;
            _sector.transform.forward = -rotated;
        }
    }
}
