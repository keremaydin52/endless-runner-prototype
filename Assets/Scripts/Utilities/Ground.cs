using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject character;

    private Vector3 _startingPosition;
    
    private float _groundpositionZ = 75f;

    private void Start()
    {
        _startingPosition = transform.position;
    }

    void Update()
    {
        if (character.transform.position.z > transform.position.z + 30)
        {
            MoveGround();
        }
    }

    void MoveGround()
    {
        transform.position += new Vector3(0, 0, _groundpositionZ);
    }

    public void ResetPosition()
    {
        transform.position = _startingPosition;
    }
}
