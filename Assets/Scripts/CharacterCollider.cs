using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterCollider : MonoBehaviour
{
	public CharacterInputController controller;
	
	public new BoxCollider collider { get { return _collider; } }
	
	private BoxCollider _collider;
    
	protected void Start()
	{
		_collider = GetComponent<BoxCollider>();
	}

	void OnTriggerEnter(Collider collider)
	{
		if (collider.tag.Equals(AppConstants.Tag.Collectable))
		{
			Collectable.collectablePool.Free(collider.gameObject.transform.parent.gameObject);
			
			//Increase score by one
			TrackManager.Instance.ChangeScore(1);
		}
		
		else if (collider.tag.Equals(AppConstants.Tag.Crate))
		{
			controller.StopSwipe();
			TrackManager.Instance.StopMove();

			StartCoroutine(CameraShaker.Instance.Shake(1f, 1f, GameOver));
		}
	}

	void GameOver()
	{
		GameManager.Instance.SwitchState("GameOver");
	}
}
