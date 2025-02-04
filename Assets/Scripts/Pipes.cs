using Andtech.StarPack;
using System;
using System.Collections;
using UnityEngine;

public class Pipes : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private float leftEdge;

    private int movingUp = 1;
    private int score;
    private Vector3 initialPosition;
    public bool isGapAdjusted;

    public bool IsGapAdjusted {  
        get { return isGapAdjusted; }
        set { isGapAdjusted = value; }
    }

    public int Score { get { return score; } set { score = value; } }

    private void Start()
    {
        leftEdge = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)).x - 1f; // completely left the scene   
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
            if ((transform.position.y >= initialPosition.y + 1f && movingUp == 1) || (transform.position.y <= initialPosition.y - 1f && movingUp == -1))
            {
                movingUp *= -1; // Reverse direction
            }
        }

        if (transform.position.x < leftEdge)
        {
            PipeSpawner pipeSpawner = FindObjectOfType<PipeSpawner>();

            pipeSpawner.ReplacePipe(gameObject);
            pipeSpawner.RemovePipe();
        }
    }
    private void UpdateScore()
    {
        score = FindObjectOfType<ScoreManager>().Score;
    }

    private void OnEnable()
    {
        UpdateScore();
        
        initialPosition = transform.position;
    }

}
