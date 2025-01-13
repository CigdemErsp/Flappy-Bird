using System;
using System.Collections;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float leftEdge;
    private Vector3 position;

    private int movingUp = 1;
    private int score;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x - 1f; // completely left the scene
        position = transform.position;
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (score > 10)
        {
            transform.position += new Vector3(0, movingUp * 2f * Time.deltaTime, 0);

            // Reverse direction when reaching the limit
            if (Mathf.Abs(transform.position.y - (position.y + 1f)) < 0.01f ||
                Mathf.Abs(transform.position.y - (position.y - 1f)) < 0.01f)
            {
                movingUp *= -1; // Reverse direction
            }
        }

        if (transform.position.x < leftEdge)
            Destroy(gameObject);
    }
    private void UpdateScore(int score)
    {
        this.score = score;
    }

    private void OnEnable()
    {
        GameManager.onScoreDelegate += UpdateScore;
    }

    private void OnDisable()
    {
        GameManager.onScoreDelegate -= UpdateScore;
    }

}
