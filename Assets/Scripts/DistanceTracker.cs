using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class DistanceTracker : MonoBehaviour
{
    #region actions
    public event Action OnDistanceThresholdReached;
    #endregion

    #region serializefields
    [SerializeField] private TMP_Text _distanceText;
    [SerializeField] private Slider _distanceTracker;
    [SerializeField] private Animator _backgroundAnimator;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    #endregion

    #region private fields
    private float _distanceTravelled;
    private float _lengthOfTracker;
    private float _animationSpeed;
    private float _distanceThreshold = 10f;
    private float _lastThreshold = 0f;
    #endregion

    public float DistanceTravelled
    {
        get 
        { 
            return _distanceTravelled; 
        }
        set
        {
            _distanceTravelled = value;
        }
    }

    private void Awake()
    {
        _distanceTravelled = 0f;
        _animationSpeed = _backgroundAnimator.speed;
        _lengthOfTracker = _obstacleSpawner.DistanceNeededToWin;
        _distanceTracker.maxValue = _lengthOfTracker + _obstacleSpawner.SpawnThreshold;
        _distanceTracker.value = 0f;
    }

    public void UpdateDistance()
    {
        _distanceTravelled += _animationSpeed * Time.deltaTime;
        _distanceText.text = ((int)_distanceTravelled).ToString();
        _distanceTracker.value = _distanceTravelled;
        
        if (IsDistanceThresholdReached() && (int)_distanceTravelled % _distanceThreshold == 0)
        {
            _lastThreshold += _distanceThreshold;
            OnDistanceThresholdReached?.Invoke();
        }
    }

    // Update distance with checkpoint value
    public void UpdateDistanceToCheckpoint(float checkpointDistance)
    {
        _distanceTravelled = checkpointDistance;
        _distanceText.text = ((int)_distanceTravelled).ToString();
        _distanceTracker.value = _distanceTravelled;

        if (_lastThreshold >= _distanceThreshold)
        {
            _lastThreshold = Mathf.FloorToInt(_distanceTravelled / _distanceThreshold) * _distanceThreshold;
        }
        else
        {
            _lastThreshold = 0f;
        }
    }

    private bool IsDistanceThresholdReached()
    {
        return _distanceTravelled > _lastThreshold + _distanceThreshold;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnUpdate += UpdateDistance;
        GameManager.Instance.OnPause -= UpdateDistance;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnUpdate -= UpdateDistance;
    }

    private void InvokeDistanceThresholdAction()
    {
        OnDistanceThresholdReached?.Invoke();
    }

}