using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFollow : MonoBehaviour
{
    [Tooltip("Target Transform for camera to follow.")]
    [SerializeField] private Transform target = null;
    [Tooltip("Position offset from target.")]
    [SerializeField] private Vector3 offset = new Vector3(0, 0, 0);

    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;
    [SerializeField] private bool followZ = true;

    void FixedUpdate()
    {
        //  Calculate where to look at:
        Vector3 fullPos = target.position + offset;
        Vector3 newPos;
        newPos.x = followX ? fullPos.x : transform.position.x;
        newPos.y = followY ? fullPos.y : transform.position.y;
        newPos.z = followZ ? fullPos.z : transform.position.z;

        //  Directly set new position:
        transform.position = newPos;
    }
}
