using UnityEngine;

public class Pipes : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float leftEdge;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x - 1f; // completely left the scene
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x < leftEdge)
            Destroy(gameObject);
    }
}
