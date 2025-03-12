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
    [SerializeField] private CircleCollider2D _circleCollider;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private PopUpManager _popUpManager;
    #endregion

    #region private fields
    private Vector3 _direction;
    private bool _superPowerActivated = false;
    private float _originalGravity;
    private float _colliderRadius;
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

    private void Awake()
    {
        _colliderRadius = _circleCollider.radius;
        _originalGravity = _rigidbody.gravityScale;
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
        _superPowerActivated = true;
        _superPower.SetActive(true);

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
        _popUpManager.OnEffectSelected += ApplyEffect;
        GameManager.Instance.OnGameOver += ResetEffects;
    }

    private void OnDisable()
    {
        _superPower.SetActive(false);
    }

    private void ApplyEffect(RoguelikeEffect roguelikeEffect)
    {
        switch (roguelikeEffect.EffectName)
        {
            case "Featherweight":
                // gravity reduced for 10 seconds
                StartCoroutine(ApplyFeatherweightEffect());
                break;
            case "Shrinking Serum":
                // hitbox ½15 smaller
                StartCoroutine(ApplyShrinkingSerumEffect());
                break;
            case "Superpower":
                StartCoroutine(ActivateSuperPower());
                break;
        }
    }

    private IEnumerator ApplyFeatherweightEffect()
    {
        _rigidbody.gravityScale = _originalGravity * 0.9f; // Reduce gravity by 10%

        yield return StartCoroutine(Countdown());

        _rigidbody.gravityScale = _originalGravity;
    }

    private IEnumerator ApplyShrinkingSerumEffect()
    {
        _circleCollider.radius = _circleCollider.radius * 0.85f;

        yield return StartCoroutine(Countdown());

        _circleCollider.radius = _colliderRadius;
    }

    private IEnumerator Countdown()
    {
        for (float countdown = 10f; countdown >= 0; countdown--)
        {
            // Wait for 1 second
            yield return new WaitForSeconds(1);
        }
    }

    private void ResetEffects()
    {
        // Reset effects in case of game over
        StopAllCoroutines();

        _circleCollider.radius = _colliderRadius;
        Vector2 originalGravity = new(0, _originalGravity);
        _rigidbody.gravityScale = _originalGravity;

        _superPowerActivated = false;
        _superPower.SetActive(false);
    }

    // used in game over
    private void ResetPos()
    {
        Vector2 position = transform.position;
        position.y = 0f;
        
        transform.SetPositionAndRotation(position, Quaternion.Euler(0, 0, 0));
    }

}
