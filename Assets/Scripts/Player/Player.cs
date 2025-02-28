using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region actions
    public event Action<int> OnCountdownUpdated;
    public event Action OnGameEnd;
    public event Action OnGameOver;
    #endregion

    #region serializefields
    [SerializeField] private GameObject _superPower;
    [SerializeField] private ScoreManager _scoreManager;
    [SerializeField] private DistanceTracker _distanceTracker;
    [SerializeField] private CheckpointManager _checkpointManager;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    #endregion

    #region private fields
    private Vector3 _direction;
    private bool _superPowerActivated = false;
    #endregion

    public bool SuperPowerActivated 
    { 
        get 
        { 
            return _superPowerActivated; 
        }
        set
        {
            _superPowerActivated = value;
        }
    }

    public void StartSmoothResetPos()
    {
        StartCoroutine(SmoothResetPos());
    }

    public void RestoreState(int newScore, int newDistance, int newCoins)
    {
        _scoreManager.Score = newScore;
        _distanceTracker.DistanceTravelled = newDistance;
        _scoreManager.CoinCount = newCoins;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Obstacle":
                HandleObstacleCollision();
                break;

            case "Ground":
                HandleGroundCollision();
                break;

            case "Coin":
                HandleCoinCollision(collision);
                break;

            case "Score":
                HandleScoreCollision();
                break;

            case "heart":
                HandleHeartCollision(collision);
                break;

            case "GameEnd":
                HandleGameEndCollision(collision);
                break;

            case "Checkpoint":
                HandleCheckpointCollision(collision);
                break;
        }
    }

    private void HandleObstacleCollision()
    {
        if (!_superPowerActivated)
            OnGameOver?.Invoke();
        else
            _scoreManager.IncreaseScore();
    }

    private void HandleGroundCollision()
    {
        OnGameOver?.Invoke();
    }

    private void HandleCoinCollision(Collider2D collision)
    {
        ActivateExplosion(collision);
        collision.gameObject.transform.parent.gameObject.transform.Find("Score Box").gameObject.SetActive(false);
        collision.gameObject.SetActive(false);

        _scoreManager.IncreaseCoinCount();
        _scoreManager.IncreaseScore();
    }

    private void HandleScoreCollision()
    {
        _scoreManager.IncreaseScore();
    }

    private void HandleHeartCollision(Collider2D collision)
    {
        ActivateExplosion(collision);
        collision.gameObject.SetActive(false);

        _superPowerActivated = true;
        _superPower.SetActive(true);
        StartCoroutine(ActivateSuperPower());
    }

    private void HandleGameEndCollision(Collider2D collision)
    {
        ActivateExplosionForFlag(collision);
        OnGameEnd?.Invoke();
    }

    private void HandleCheckpointCollision(Collider2D collision)
    {
        ActivateExplosionForFlag(collision);

        _checkpointManager.SaveCheckpoint(
            _scoreManager.Score,
            (int)_distanceTracker.DistanceTravelled,
            _scoreManager.CoinCount,
            _obstacleSpawner.NumberOfCheckpoints
        );
    }

    private void ActivateExplosion(Collider2D collision)
    {
        collision.gameObject.transform.parent.gameObject.transform.Find("Explosion").gameObject.SetActive(true);
    }

    private void ActivateExplosionForFlag(Collider2D collision)
    {
        collision.gameObject.transform.Find("Explosion").gameObject.SetActive(true);
    }

    private IEnumerator ActivateSuperPower()
    {
        Time.timeScale = 1f;

        for (float countdown = 10f; countdown >= 0; countdown--)
        {
            OnCountdownUpdated?.Invoke((int)countdown);
            // Wait for 1 second
            yield return new WaitForSeconds(1);
        }

        OnCountdownUpdated?.Invoke(-1);
        _superPowerActivated = false;
        _superPower.SetActive(false);
    }

    private void OnEnable()
    {
        ResetPos();
    }

    private void OnDisable()
    {
        _superPower.gameObject.SetActive(false);
    }

    // used in game over
    private void ResetPos()
    {
        Vector2 position = transform.position;
        position.y = 0f;
        transform.position = position;

        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    // used in game end
    private IEnumerator SmoothResetPos()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new(transform.position.x, 0f, transform.position.z);
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        float duration = 1f; // Duration in seconds
        int steps = 60; // Number of steps (frames) for the transition
        float stepDuration = duration / steps;

        for (int i = 0; i <= steps; i++)
        {
            float t = Mathf.Clamp01((float)i / steps);

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return new WaitForSeconds(stepDuration);
        }

        // Ensure the final state is exactly set.
        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

}
