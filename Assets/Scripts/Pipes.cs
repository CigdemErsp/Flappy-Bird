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
    private Vector3 initialPosition;

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x - 1f; // completely left the scene
        position = transform.position;
        initialPosition = transform.position;
        UpdateScore();
    }

    private void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        // Check if score is greater than 10 for vertical movement
        if (score >= 10)
        {
            // Move vertically up and down
            transform.position += new Vector3(0, movingUp * 2f * Time.deltaTime, 0);

            // Reverse direction when reaching the vertical limits
            if (transform.position.y >= initialPosition.y + 1f || transform.position.y <= initialPosition.y - 1f)
            {
                movingUp *= -1; // Reverse direction
            }
        }

        // Destroy the pipe and remove from the spawner if it goes out of bounds
        if (transform.position.x < leftEdge)
        {
            Destroy(gameObject);
            FindObjectOfType<PipeSpawner>().removePipe();
        }
    }
    private void UpdateScore()
    {
        score = FindObjectOfType<ScoreManager>().getScore();
    }

}
