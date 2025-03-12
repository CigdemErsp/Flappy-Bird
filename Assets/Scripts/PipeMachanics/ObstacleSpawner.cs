using System;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    #region const fields
    private const int _distanceNeededToWin = 15;
    private const float _minimumDistanceBetweenCheckpoints = 10f;
    #endregion

    #region serializefields
    [SerializeField] private GameObject _checkpoint;
    [SerializeField] private List<GameObject> _pipesPrefabs;
    [SerializeField] private GameObject _endFlag;
    [SerializeField] private DistanceTracker _distanceTracker;
    [SerializeField] private Player _player;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private CheckpointData _checkpointData;
    
    [SerializeField] private float _minHeight = -1f;
    [SerializeField] private float _maxHeight = 1f;
    #endregion

    #region private fields
    private int _score;

    private List<int> _allPipes;

    private float _lastSpawnDistance = 0f;
    private float _spawnThreshold = 2f;
     
    private int _numberOfMaxCheckpoints;
    private int _numberOfCheckpoints;
    #endregion

    public int DistanceNeededToWin {  get { return _distanceNeededToWin; } }

    public float SpawnThreshold { get { return _spawnThreshold; } }

    public int NumberOfCheckpoints {  get { return _numberOfCheckpoints; } set { _numberOfCheckpoints = value; } }

    private void Awake()
    {
        _allPipes = new List<int>();

        _numberOfMaxCheckpoints = Mathf.FloorToInt(_distanceNeededToWin / _minimumDistanceBetweenCheckpoints) - 1;
    }

    public void ReplacePipe(GameObject pipeGameObject)
    {
        Pipes pipe = pipeGameObject.GetComponent<Pipes>();

        pipe.enabled = false;
        pipeGameObject.transform.position = new Vector3(10, 0, 0);

        Transform scoreBox = pipe.Coin;

        Transform explosion = pipe.Explosion;
        Transform score = pipe.ScoreBox;

        scoreBox.gameObject.SetActive(true);
        explosion.gameObject.SetActive(false);
        score.gameObject.SetActive(true);
    }

    public void ReplaceFlag(GameObject flagGameObject)
    {
        FlagBase flagBase = flagGameObject.GetComponent<FlagBase>();

        Transform explosion = flagBase.Explosion;

        flagBase.enabled = false;
        flagGameObject.transform.position = new Vector3(10, -3.21000004f, 0);
        explosion.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        GameManager.Instance.OnUpdate += Spawn;
        GameManager.Instance.OnPause -= Spawn;
        _lastSpawnDistance = 0f;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnUpdate -= Spawn;

        if (!gameObject.activeInHierarchy) return;

        ResetPipesAndFlags();
        ResetPipeGaps();
    }

    private void ResetPipesAndFlags()
    {
        foreach (var pipe in _pipesPrefabs)
        { 
            ReplacePipe(pipe);
        }

        ReplaceFlag(_checkpoint);
        ReplaceFlag(_endFlag);
    }

    private void ResetPipeGaps()
    {
        foreach (var pipe in _pipesPrefabs)
        {
            if (pipe.GetComponent<Pipes>().IsGapAdjusted)
            {
                pipe.GetComponent<Pipes>().ResetGap(pipe);
            }
        }
    }

    private void Spawn()
    {
        if (!gameObject.activeInHierarchy) return;

        if (_distanceTracker.DistanceTravelled - _lastSpawnDistance >= _spawnThreshold || IsGameStarted())
        {
            UpdateScore();

            if (_distanceTracker.DistanceTravelled >= _distanceNeededToWin)
            {
                _ = HandleGameEnd();
            }
            else if (IsNextCheckpointDue())
            {
                _ = HandleCheckpoint();
            }
            else
            {
                _ = HandlePipe();
            }

            _lastSpawnDistance = _distanceTracker.DistanceTravelled;
        }
    }

    private bool IsGameStarted()
    {
        return _distanceTracker.DistanceTravelled - _checkpointData.Distance == 0;
    }

    private GameObject HandleGameEnd()
    {
        GameObject pipe = _endFlag;
        pipe.GetComponent<EndFlag>().enabled = true;

        GameManager.Instance.OnUpdate -= Spawn;

        return pipe;
    }

    private GameObject HandleCheckpoint()
    {
        _numberOfCheckpoints++;
        GameObject pipe = _checkpoint;
        pipe.GetComponent<Checkpoint>().enabled = true;

        return pipe;
    }

    private GameObject HandlePipe()
    {
        GameObject pipe;
        pipe = SpawnPipe();

        pipe.GetComponent<Pipes>().enabled = true;
        AdjustPipeGapIfNeeded(pipe);
        return pipe;
    }

    private GameObject SpawnPipe()
    {
        _pipesPrefabs[0].transform.position += Vector3.up * UnityEngine.Random.Range(_minHeight, _maxHeight);
        GameObject pipe = _pipesPrefabs[0];
        _pipesPrefabs.RemoveAt(0); // removed, will be added
        _pipesPrefabs.Add(pipe);
        UpdatePipes(0);

        return pipe;
    }

    private void AdjustPipeGapIfNeeded(GameObject pipe)
    {
        if (_score >= 5 && !pipe.GetComponent<Pipes>().IsGapAdjusted)
        {
            pipe.GetComponent<Pipes>().NarrowGap(pipe);
            _score++;
        }
    }

    private bool IsNextCheckpointDue()
    {
        bool isNumberOfCheckpointsEnough = _numberOfCheckpoints <= _numberOfMaxCheckpoints;

        float nextCheckpointDistance = (_numberOfCheckpoints + 1) * _minimumDistanceBetweenCheckpoints;

        return isNumberOfCheckpointsEnough &&
                (_distanceTracker.DistanceTravelled > nextCheckpointDistance - 1 &&
                _distanceTracker.DistanceTravelled < nextCheckpointDistance + 1);
    }

    private void UpdatePipes(int i)
    {
        _allPipes.Add(i);
    }

    public void RemovePipe()
    {
        _allPipes.RemoveAt(0);
    }

    public void AddPipe(GameObject pipe)
    {
        _pipesPrefabs.Add(pipe);
    }
    private void UpdateScore()
    {
        _score = _scoreManager.Score;
    }

}
