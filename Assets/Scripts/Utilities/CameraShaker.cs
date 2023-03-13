using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using Random = UnityEngine.Random;

public class CameraShaker : Singleton<CameraShaker>
{
#if UNITY_EDITOR
    private void Update()
    {
        // Test camera shake
        if (Input.GetKey(KeyCode.S))
        {
            StartCoroutine(Shake(1f, 1f, null));
        }
    }
#endif

    public IEnumerator Shake(float duration, float magnitude, Action callback)
    {
        Vector3 originalPos = transform.localPosition;
        
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1, 1) * magnitude;
            float y = Random.Range(-1, 1) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPos;
        callback?.Invoke();
    }
}
