using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;

    private Vector3 _offset;

    void FixedUpdate ()
    {
        Vector3 desiredPosition = target.position + _offset;
        transform.position = desiredPosition;

        //transform.LookAt(target);
    }
}
