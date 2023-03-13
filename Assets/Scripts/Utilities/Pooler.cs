using UnityEngine;
using System.Collections.Generic;

public class Pooler
{
    private Stack<GameObject> freeInstances = new Stack<GameObject>();
    private GameObject original;

    public Pooler(GameObject original, int initialSize)
    {
        this.original = original;
        freeInstances = new Stack<GameObject>(initialSize);

        for (int i = 0; i < initialSize; ++i)
        {
            GameObject obj = Object.Instantiate(original);
            obj.SetActive(false);
            freeInstances.Push(obj);
        }
    }

    public GameObject Get(Vector3 pos, Quaternion quat)
    {
        GameObject ret = freeInstances.Count > 0 ? freeInstances.Pop() : Object.Instantiate(original);

        ret.SetActive(true);
        ret.transform.position = pos;
        ret.transform.rotation = quat;

        return ret;
    }

    public void Free(GameObject obj)
    {
        obj.transform.SetParent(null);
        obj.SetActive(false);
        freeInstances.Push(obj);
    }
}