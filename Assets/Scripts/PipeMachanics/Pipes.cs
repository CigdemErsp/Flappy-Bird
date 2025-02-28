using Andtech.StarPack;
using System;
using System.Collections;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    #region serializefields
    [SerializeField] private Animator _backgroundAnimator;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private ScoreManager _scoreManager;

    [SerializeField] private Transform _upperPipe;
    [SerializeField] private Transform _bottomPipe;
    [SerializeField] private Transform _scoreBox;
    [SerializeField] private Transform _coin;
    [SerializeField] private Transform _heart;
    [SerializeField] private Transform _explosion;
    #endregion

    #region private fields
    private float _speed;
    private int _movingUp = 1;
    private int _score;
    private Vector3 _initialPosition;
    private bool _isGapAdjusted;
    private float _offset;
    #endregion

    public Transform Coin { get {  return _coin; } }

    public Transform Heart { get { return _heart; } }

    public Transform Explosion { get { return _explosion; } }

    public Transform ScoreBox { get { return _scoreBox; } }

    public bool IsGapAdjusted {  
        get { return _isGapAdjusted; }
        set { _isGapAdjusted = value; }
    }

    public int Score { get { return _score; } set { _score = value; } }

    public void NarrowGap(GameObject pipe)
    {
        System.Random random = new System.Random();
        _offset = (float)(0.1 + random.NextDouble() * 0.4); // generate random double from 0.1 to 0.5

        pipe.GetComponent<Pipes>().IsGapAdjusted = true;
        AdjustPipePositions(-1 * (_offset));
        AdjustScoreBoxScale(_offset);
    }

    public void ResetGap(GameObject pipe)
    {
        pipe.GetComponent<Pipes>().IsGapAdjusted = false;
        AdjustPipePositions(_offset);
        AdjustScoreBoxScale(-1 * (_offset));
    }

    private void AdjustPipePositions(float offset)
    {
        _upperPipe.position = new Vector3(_upperPipe.position.x, _upperPipe.position.y + offset, _upperPipe.position.z);
        _bottomPipe.position = new Vector3(_bottomPipe.position.x, _bottomPipe.position.y - offset, _bottomPipe.position.z);
    }

    private void AdjustScoreBoxScale(float offset)
    {
        _scoreBox.localScale = new Vector3(_scoreBox.localScale.x, _scoreBox.localScale.y + offset, _scoreBox.localScale.z);
    }

    private void MovePipeVertically()
    {
        _speed = _backgroundAnimator.speed * 5;
        transform.position += _speed * Time.deltaTime * Vector3.left;

        // Check if score is greater than 10 for vertical movement
        if (_score >= 10)
        {
            // Move vertically up and down
            transform.position += new Vector3(0, _movingUp * 2f * Time.deltaTime, 0);

            // Reverse direction when reaching the vertical limits
            if (CheckIfPipeInLimit())
            {
                _movingUp *= -1; // Reverse direction
            }
        }
    }

    private bool CheckIfPipeInLimit()
    {
        bool upperLimit = (transform.position.y >= _initialPosition.y + 1f) && (_movingUp == 1);
        bool lowerLimit = (transform.position.y <= _initialPosition.y - 1f) && (_movingUp == -1);

        return upperLimit || lowerLimit;
    }

    private void UpdateScore()
    {
        _score = _scoreManager.Score;
    }

    private void OnEnable()
    {
        GameManager.Instance.OnUpdate += MovePipeVertically;
        UpdateScore();
        _initialPosition = transform.position;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnUpdate -= MovePipeVertically;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SceneEnd"))
        {
            _obstacleSpawner.ReplacePipe(gameObject);
            _obstacleSpawner.RemovePipe();
        }
    }

}
