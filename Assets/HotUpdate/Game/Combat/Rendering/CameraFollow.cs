using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform Target;

    void LateUpdate()
    {
        if (Target != null)
        {
            transform.position = Target.position;
        }
    }
}
