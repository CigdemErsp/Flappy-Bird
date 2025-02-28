using UnityEngine;
using TMPro;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    #region instance
    public static GameManager Instance { get; private set; }
    #endregion

    #region actions
    public event Action OnUpdate;
    #endregion

    #region serialize fields
    [SerializeField] private TMP_Text _countdownText;
    [SerializeField] private TMP_Text _superPowerCountdown;

    [SerializeField] private GameObject _playButton;
    [SerializeField] private GameObject _gameOver;
    [SerializeField] private GameObject _getReady;

    [SerializeField] private Player _player;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private CheckpointData _checkpointData;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private LeaderboardManager _leaderboardManager;
    [SerializeField] private CheckpointManager _checkpointManager;
    [SerializeField] private DistanceTracker _distanceTracker;
    [SerializeField] private List<Pipes> _pipes;

    #endregion

    private bool _isGameStarted;

    private void Awake()
    {
        HandleSingleton();

        InitializeSettings();
        InitializePlayer();
        InitializeGame();
    }

    private void Update()
    {
        if (_isGameStarted)
            OnUpdate?.Invoke();
    }

    private void HandleSingleton()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void InitializeSettings()
    {
        Application.targetFrameRate = 60;
    }

    private void InitializePlayer()
    {
        _player.enabled = false;
        _player.OnCountdownUpdated += UpdateCountdownText;
    }

    private void InitializeGame()
    {
        _gameOver.SetActive(false);
        _obstacleSpawner.enabled = false;
        _isGameStarted = false;
    }

    public void OnClick()
    {
        StopPlayerMovement();
        LoadCheckpointData();
        DisableGameElements();
        StartCountdown();
        StartGame();
    }

    private void StopPlayerMovement()
    {
        _player.GetComponent<Rigidbody2D>().simulated = false;
    }

    private void LoadCheckpointData()
    {
        _checkpointManager.LoadCheckpoint(_player.gameObject);
        _distanceTracker.UpdateDistanceToCheckpoint(_checkpointData.Distance);
        _scoreManager.UpdateScoreManagerToCheckpoint(_checkpointData.PlayerScore, _checkpointData.PlayerCoin);
    }

    private void DisableGameElements()
    {
        _leaderboardManager.enabled = false;
        _scoreManager.ScoreText.gameObject.SetActive(false);
        _obstacleSpawner.enabled = false;
        _getReady.SetActive(true);
        _countdownText.gameObject.SetActive(true);
    }

    private void StartCountdown()
    {
        _player.enabled = true;
    }

    private void StartGame()
    {
        StartCoroutine(OnStart());
    }

    public IEnumerator OnStart()
    {
        foreach (var pipe in _pipes)
        {
            pipe.Score = _scoreManager.Score;
        }

        _playButton.SetActive(false);
        _gameOver.SetActive(false);

        Time.timeScale = 1f;

        // Countdown from 3 to 1
        for (float countdown = 3f; countdown > 0; countdown--)
        {
            _countdownText.text = countdown.ToString();
            yield return new WaitForSeconds(1);
        }

        _countdownText.gameObject.SetActive(false);
        Play();
    }


    public void Play()
    {
        _isGameStarted = true;
        _distanceTracker.enabled = true;
        _obstacleSpawner.enabled = true;
        _getReady.SetActive(false);
        _scoreManager.ScoreText.gameObject.SetActive(true);

        _player.enabled = true;
        _player.GetComponent<Rigidbody2D>().simulated = true;
    }

    public void Pause()
    {
        _isGameStarted = false;
        _distanceTracker.enabled = false;
        _player.enabled = false;
        _player.GetComponent<Rigidbody2D>().simulated = false;
        Time.timeScale = 0f;
    }

    public void GameOver()
    {
        _scoreManager.CheckHighscore();
        _gameOver.SetActive(true);
        _playButton.SetActive(true);
        _superPowerCountdown.gameObject.SetActive(false);
        _player.StopAllCoroutines();
        _player.SuperPowerActivated = false;

        _leaderboardManager.enabled = true;

        Pause();
    }

    private void UpdateCountdownText(int countdown)
    {
        if (countdown >= 0)
        {
            _superPowerCountdown.text = countdown.ToString();
            if (!_superPowerCountdown.IsActive())
            {
                _superPowerCountdown.gameObject.SetActive(true);
            }
        }
        else
        {
            _superPowerCountdown.gameObject.SetActive(false);
        }
    }

    private void GameEnd()
    {
        _distanceTracker.enabled = false;
        _scoreManager.CheckHighscore();
        _superPowerCountdown.gameObject.SetActive(false);
        _player.StopAllCoroutines();
        _player.SuperPowerActivated = false;
        _player.StartSmoothResetPos();
        _player.GetComponent<Rigidbody2D>().simulated = false;

        _leaderboardManager.enabled = true;
        _scoreManager.ScoreText.gameObject.SetActive(false);
        _obstacleSpawner.enabled = false;

        _player.enabled = false;
    }

    private void OnEnable()
    {
        Time.timeScale = 0f;
        _player.OnGameEnd += GameEnd;
        _player.OnGameOver += GameOver;
    }

}