using UnityEngine;

public class Collectable : MonoBehaviour
{
    public static Pooler collectablePool;
    
    private void Update()
    {
        if (transform.position.z < TrackManager.Instance.character.transform.position.z - 15f)
        {
            collectablePool.Free(gameObject);
        }
    }
}
