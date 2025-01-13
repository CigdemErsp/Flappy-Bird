using System.Collections;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipe;
    [SerializeField] private float spawnRate = 1f;
    [SerializeField] private float minHeight = -1f;
    [SerializeField] private float maxHeight = 1f;

    private int score;

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
        GameManager.onScoreDelegate += UpdateScore;
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        GameObject pipes = Instantiate(pipe, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * Random.Range(minHeight, maxHeight);

        if (score > 5)
        {
            Transform upperPipe = pipes.transform.Find("Upper Pipe");
            Transform bottomPipe = pipes.transform.Find("Bottom Pipe");
            Transform scoreBox = pipes.transform.Find("Score Box");

            Vector3 upperPipeCurrentPosition = upperPipe.position;
            Vector3 bottomPipeCurrentPosition = bottomPipe.position;
            Vector3 scoreBoxLocalScale = scoreBox.localScale;

            // Modify only the y component and keep the x and z components unchanged
            upperPipe.position = new Vector3(upperPipeCurrentPosition.x, upperPipeCurrentPosition.y - 0.5f, upperPipeCurrentPosition.z);
            bottomPipe.position = new Vector3(bottomPipeCurrentPosition.x, bottomPipeCurrentPosition.y + 0.5f, bottomPipeCurrentPosition.z);
            scoreBox.localScale = new Vector3(scoreBoxLocalScale.x, scoreBoxLocalScale.y + 0.5f, scoreBoxLocalScale.z);
        }        
    }

    private void UpdateScore(int score)
    {
        this.score = score;
    }
}
