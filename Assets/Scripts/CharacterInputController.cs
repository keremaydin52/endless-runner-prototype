using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInputController : MonoBehaviour
{
    public TrackManager trackManager;
    public CharacterCollider characterCollider;
    
    public float slideLength = 2.0f;

    private int _collectables;
    
    private bool _canSwipe;
    private bool _sliding;
    
    private float _slideStart;
    private float _swipeThreshold = 10f;
    private float _laneSwipeDuration = 0.10f;
    
    private int _currentLane = _startingLane;
    private Vector3 _targetPosition = Vector3.zero;

    private readonly Vector3 _startingPosition = Vector3.forward * 2f;

    private const int _startingLane = 0;
    
    private Vector2 _initialSwipePos ;
    
    public int Collectables { get { return _collectables; } set { _collectables = value; } }
    
    void Update ()
    {
        if( Input.GetMouseButtonDown(0) )
        {
            _initialSwipePos = Input.mousePosition;
        }
        if( Input.GetMouseButtonUp(0))
        {       
            Swipe(Input.mousePosition);
        }
    }
    
    void Swipe(Vector2 finalPos)
    {
        Vector2 diff = finalPos - _initialSwipePos;
        
        if(Mathf.Abs(diff.y) < _swipeThreshold) return;
        
        if (diff.y < 0)
        {
            ChangeLane(1);
        }
        else if (diff.y > 0)
        {
            ChangeLane(-1);
        }
    }
    
    public void Init()
    {
        transform.position = _startingPosition;
        _targetPosition = Vector3.zero;

        _currentLane = _startingLane;
        characterCollider.transform.localPosition = Vector3.zero;
    }

    public void StartRunning()
    {   
        Init();
        StartMoving();
    }

    public void StartMoving()
    {
        _canSwipe = true;
    }
    
    public void StopSwipe()
    {
        _canSwipe = false;
    }

    void ChangeLane(int direction)
    {
        if (!_canSwipe) return;

        int targetLane = _currentLane + direction;

        // Ignore, we are on the borders.
        if (targetLane < -2 || targetLane > 2) return;

        _currentLane = targetLane;
        _targetPosition = new Vector3(_currentLane * trackManager.laneOffset, transform.position.y, transform.position.z);
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        float startX = transform.position.x;
        float endX = _targetPosition.x;
        float timePassed = 0f;
        while (timePassed < _laneSwipeDuration)
        {
            timePassed += Time.deltaTime;
            float t = timePassed / _laneSwipeDuration;
            float currentX = Mathf.Lerp(startX, endX, t);
            transform.position = new Vector3(currentX, transform.position.y, transform.position.z);
            yield return null;
        }
    }
}
