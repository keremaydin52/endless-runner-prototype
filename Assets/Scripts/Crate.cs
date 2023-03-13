using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate : MonoBehaviour
{
    public static Pooler cratePool;
    
    private void Update()
    {
        if (transform.position.z < TrackManager.Instance.character.transform.position.z - 15f)
        {
            cratePool.Free(gameObject);
        }
    }
}
