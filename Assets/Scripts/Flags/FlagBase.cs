using UnityEngine;
using UnityEngine.SceneManagement;

public class FlagBase : MonoBehaviour
{
    #region serializefields
    [SerializeField] private Animator _backgroundAnimator;
    [SerializeField] private ObstacleSpawner _obstacleSpawner;
    [SerializeField] private Transform _explosion;
    #endregion

    #region private fields
    private float _speed;
    #endregion

    public Transform Explosion { get { return _explosion; } }

    public void MoveFlag()
    {
        SetSpeed();
        transform.position += _speed * Time.deltaTime * Vector3.left;
    }

    private void SetSpeed()
    {
        _speed = _backgroundAnimator.speed * 5;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "SceneEnd")
        {
            _obstacleSpawner.ReplaceFlag(gameObject);
        }
    }

    private void OnEnable()
    {
        GameManager.Instance.OnUpdate += MoveFlag;
        GameManager.Instance.OnPause -= MoveFlag;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnUpdate -= MoveFlag;
    }
}
