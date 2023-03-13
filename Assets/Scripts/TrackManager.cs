using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class TrackManager : Singleton<TrackManager>
{
    public GameObject character;
    public GameObject collectable;
    public GameObject obstacle;
    public CharacterInputController characterInputController;

    public List<Ground> grounds;
    
    public bool IsLoaded { get; set; }

    private Rigidbody characterRb;

    private float _currentSegmentDistance;
    private float _totalWorldDistance;
    private bool _isMoving;
    private float _speed;
    
    public float minSpeed = 50.0f;
    public float maxSpeed = 200.0f;
    public float laneOffset = 2.0f;

    private int _leftMostLaneIndex = -2;
    private int _rightMostLaneIndex = 2;
    
    private bool _rerun;

    private List<GameObject> _activeCrates = new List<GameObject>();
    private List<GameObject> _activeCollectables = new List<GameObject>();

    Vector3 _characterOriginalPos = Vector3.zero;
    
    const float _floatingOriginThreshold = 1000f;
    const float _acceleration = 2f;
    const int _startingCollectablePoolSize = 5;
    const int _startingCratePoolSize = 5;
    
    public int Score { get; set; }

    private void Start()
    {
        Collectable.collectablePool = new Pooler(collectable, _startingCollectablePoolSize);
        Crate.cratePool = new Pooler(obstacle, _startingCratePoolSize);
        
        characterRb = character.GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_speed < maxSpeed) _speed += _acceleration * Time.deltaTime;
        else _speed = maxSpeed;

        if (_isMoving)
        {
            characterRb.velocity = new Vector3(0f, 0f, _speed);
        }
        else
        {
            characterRb.velocity = Vector3.zero;
        }
        print("speed: " + _speed);
        PreventFloatOverFlow();
    }

    public void StartMove(bool isRestart = true)
    {
        characterInputController.StartMoving();
        _isMoving = true;
        if (isRestart) _speed = minSpeed;
        
    }

    public void StopMove()
    {
        _isMoving = false;
    }
    
    public void Begin()
    {
        character.transform.position = _characterOriginalPos;
        if (!_rerun)
        {
            gameObject.SetActive(true);
            characterInputController.gameObject.SetActive(true);
            characterInputController.Collectables = 0;

            Score = 0;
        }

        FreeAllObjects();
        ResetGroundPositions();
        characterInputController.StartRunning();
        StartMove();
        IsLoaded = true;
        
        InvokeRepeating(nameof(SpawnObstacle), 0f, 0.75f);
        InvokeRepeating(nameof(SpawnCollectable), 0f, 2f);
    }

    public void CancelGenerateMethods()
    {
        CancelInvoke();
    }

    public void SpawnObstacle()
    {
        int laneIndex = Random.Range(_leftMostLaneIndex, _rightMostLaneIndex + 1);
        Vector3 spawnPosition = new Vector3(laneOffset * laneIndex, 0f, character.transform.position.z + 30);
        GameObject go = Crate.cratePool.Get(spawnPosition, Quaternion.identity);
        _activeCrates.Add(go);
    }

    public void SpawnCollectable()
    {
        int laneIndex = Random.Range(_leftMostLaneIndex, _rightMostLaneIndex + 1);
        Vector3 spawnPosition = new Vector3(laneOffset * laneIndex, 0f, character.transform.position.z + 30);
        GameObject go = Collectable.collectablePool.Get(spawnPosition, Quaternion.identity);
        _activeCollectables.Add(go);
    }

    public void FreeAllObjects()
    {
        //Free all obstacles and collectables before new game starts 
        foreach (GameObject go in _activeCollectables)
        {
            Collectable.collectablePool.Free(go);
        }

        foreach (GameObject go in _activeCrates)
        {
            Crate.cratePool.Free(go);
        }
    }
    
    void ResetGroundPositions()
    {
        foreach (var ground in grounds)
        {
            ground.ResetPosition();
        }
    }

    public void PreventFloatOverFlow()
    {
        if(character.transform.position.z < _floatingOriginThreshold) return;
        
        MoveBack(character.transform);
        foreach (GameObject go in _activeCollectables)
        {
            MoveBack(go.transform);
        }
        
        foreach (GameObject go in _activeCrates)
        {
            MoveBack(go.transform);
        }
        
        foreach (Ground go in grounds)
        {
            MoveBack(go.transform);
        }
    }

    public void MoveBack(Transform transform)
    {
        transform.position = new Vector3(transform.position.x, transform.position.y,
            transform.position.z - _floatingOriginThreshold);
    }

    public void ChangeScore(int amount)
    {
        Score += amount;
    }
}
