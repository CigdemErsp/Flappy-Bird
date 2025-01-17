using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] pipesPrefab;
    private float spawnRate = 2f;
    [SerializeField] private float minHeight = -1f;
    [SerializeField] private float maxHeight = 1f;

    private int score;

    private List<int> allPipes;

    private void Awake()
    {
        allPipes = new List<int>();
    }

    private void OnEnable()
    {
        InvokeRepeating(nameof(Spawn), spawnRate, spawnRate);
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
    }

    private void Spawn()
    {
        if(score < 5)
            UpdateScore();

        int index = 0;
     
        if (allPipes.Count != 0 && allPipes.Contains(1))
        {
            // Debug.Log(allPipes.Count);
           
            index = 0;
            
        }
        else if(FindObjectOfType<Player>().superPowerActivated)
        {
            index = 0;
        }
        else
        {
            int rand = UnityEngine.Random.Range(0, 2);
            
            if (rand == 0)
            {
                index = 1;
            }
        }

        GameObject pipe = pipesPrefab[index];
        updatePipes(index);
        GameObject pipes = Instantiate(pipe, transform.position, Quaternion.identity);
        pipes.transform.position += Vector3.up * UnityEngine.Random.Range(minHeight, maxHeight);

        if (score >= 5)
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

    private void updatePipes(int i)
    {
        allPipes.Add(i);
    }

    public void removePipe()
    {
        allPipes.RemoveAt(0);
    }
    private void UpdateScore()
    {
        score = FindObjectOfType<ScoreManager>().getScore();
    }
}
