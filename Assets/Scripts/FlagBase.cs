using UnityEngine;

public class FlagBase : MonoBehaviour
{
    [SerializeField] private Animator backgroundAnimator;

    private float speed;
    private float leftEdge;

    private void Awake()
    {
        speed = backgroundAnimator.speed * 5;
    }

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x - 2f; // completely left the scene   
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftEdge)
        {
            PipeSpawner pipeSpawner = FindObjectOfType<PipeSpawner>();

            pipeSpawner.ReplaceFlag(gameObject);
        }
    }
}
