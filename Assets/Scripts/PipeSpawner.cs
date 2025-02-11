using System;
using System.Collections.Generic;
using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject pipesPrefabWithHeart;
    [SerializeField] private List<GameObject> pipesPrefabs;
    [SerializeField] private GameObject endFlag;
    
    [SerializeField] private float minHeight = -1f;
    [SerializeField] private float maxHeight = 1f;

    private int score;

    private List<int> allPipes;

    private float lastSpawnDistance = 0f;
    private float spawnThreshold = 3f; // Spawn every 3 distance traveled
    private DistanceTracker distanceTracker;
    private int distanceNeededToWin = 12;

    public int DistanceNeededToWin {  get { return distanceNeededToWin; } }

    public float SpawnThreshold { get { return spawnThreshold; } }

    private void Update()
    {
        if (distanceTracker.GetDistanceTraveled() - lastSpawnDistance >= spawnThreshold)
        {
            Debug.Log("sasas");
            Spawn();
            lastSpawnDistance = distanceTracker.GetDistanceTraveled();
        }
    }

    private void Awake()
    {
        distanceTracker = FindObjectOfType<DistanceTracker>();
        allPipes = new List<int>();
    }

    private void OnEnable()
    {
        lastSpawnDistance = 0f;
        Spawn();
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(Spawn));
        if (!gameObject.activeInHierarchy) return;
        ReplacePipe(pipesPrefabWithHeart);

        if(pipesPrefabWithHeart.GetComponent<Pipes>().IsGapAdjusted)
            ResetGap(pipesPrefabWithHeart);

        foreach (var pipe in pipesPrefabs)
        {
            ReplacePipe(pipe);
            if (pipe.GetComponent<Pipes>().IsGapAdjusted)
                ResetGap(pipe);
        }
    }

    private void Spawn()
    {
        UpdateScore();

        GameObject pipe;
        int superPowerNotExists = 1; // if 0 = there is heart in scene/superpower is activated, do not add another pipe with heart to scene

        if ((allPipes.Count != 0 && allPipes.Contains(1)) || FindObjectOfType<Player>().superPowerActivated)
        {
            superPowerNotExists = 0;
        }

        if (distanceTracker.GetDistanceTraveled() >= distanceNeededToWin)
        {
            pipe = endFlag;
            pipe.GetComponent<EndFlag>().enabled = true;
            CancelInvoke(nameof(Spawn));
        }
        else
        {
            if (superPowerNotExists == 0)
            {
                pipe = pipesPrefabs[0];
                pipe.transform.position += Vector3.up * UnityEngine.Random.Range(minHeight, maxHeight);
                pipesPrefabs.RemoveAt(0); // removed, will be added
                pipesPrefabs.Add(pipe);
                UpdatePipes(0);
            }
            else
            {
                System.Random random = new System.Random();
                if (random.Next(2) == 0) // spawn heart
                {
                    pipe = pipesPrefabWithHeart;
                    pipe.transform.position += Vector3.up * UnityEngine.Random.Range(minHeight, maxHeight);
                    UpdatePipes(1);
                }
                else
                {
                    pipe = pipesPrefabs[0];
                    pipe.transform.position += Vector3.up * UnityEngine.Random.Range(minHeight, maxHeight);
                    pipesPrefabs.RemoveAt(0); // removed, will be added
                    pipesPrefabs.Add(pipe);
                    UpdatePipes(0);
                }
            }

            pipe.GetComponent<Pipes>().enabled = true;

            if (score >= 5 && !pipe.GetComponent<Pipes>().IsGapAdjusted)
            {
                NarrowGap(pipe);
                score++;
            }
        }
    }

    private void NarrowGap(GameObject pipe)
    {
        pipe.GetComponent<Pipes>().IsGapAdjusted = true;
        Transform upperPipe = pipe.transform.Find("Upper Pipe");
        Transform bottomPipe = pipe.transform.Find("Bottom Pipe");
        Transform scoreBox = pipe.transform.Find("Score Box");

        Vector3 upperPipeCurrentPosition = upperPipe.position;
        Vector3 bottomPipeCurrentPosition = bottomPipe.position;
        Vector3 scoreBoxLocalScale = scoreBox.localScale;

        // Modify only the y component and keep the x and z components unchanged
        upperPipe.position = new Vector3(upperPipeCurrentPosition.x, upperPipeCurrentPosition.y - 0.5f, upperPipeCurrentPosition.z);
        bottomPipe.position = new Vector3(bottomPipeCurrentPosition.x, bottomPipeCurrentPosition.y + 0.5f, bottomPipeCurrentPosition.z);
        scoreBox.localScale = new Vector3(scoreBoxLocalScale.x, scoreBoxLocalScale.y + 0.5f, scoreBoxLocalScale.z);   
    }

    private void ResetGap(GameObject pipe)
    {
        pipe.GetComponent<Pipes>().IsGapAdjusted = false;
        Transform upperPipe = pipe.transform.Find("Upper Pipe");
        Transform bottomPipe = pipe.transform.Find("Bottom Pipe");
        Transform scoreBox = pipe.transform.Find("Score Box");

        Vector3 upperPipeCurrentPosition = upperPipe.position;
        Vector3 bottomPipeCurrentPosition = bottomPipe.position;
        Vector3 scoreBoxLocalScale = scoreBox.localScale;

        // Modify only the y component and keep the x and z components unchanged
        upperPipe.position = new Vector3(upperPipeCurrentPosition.x, upperPipeCurrentPosition.y + 0.5f, upperPipeCurrentPosition.z);
        bottomPipe.position = new Vector3(bottomPipeCurrentPosition.x, bottomPipeCurrentPosition.y - 0.5f, bottomPipeCurrentPosition.z);
        scoreBox.localScale = new Vector3(scoreBoxLocalScale.x, scoreBoxLocalScale.y - 0.5f, scoreBoxLocalScale.z);
    }

    private void UpdatePipes(int i)
    {
        allPipes.Add(i);
    }

    public void RemovePipe()
    {
        allPipes.RemoveAt(0);
    }

    public void AddPipe(GameObject pipe)
    {
        pipesPrefabs.Add(pipe);
    }
    private void UpdateScore()
    {
        score = FindObjectOfType<ScoreManager>().Score;
    }

    public void ReplacePipe(GameObject pipe)
    {
        pipe.GetComponent<Pipes>().enabled = false;
        pipe.transform.position = new Vector3(10,0,0);

        Transform scoreBox = pipe.transform.Find("Coin");
        if (scoreBox == null)
        {
            scoreBox = pipe.transform.Find("Heart11");
        }

        Transform explosion = pipe.transform.Find("Explosion");
        Transform score = pipe.transform.Find("Score Box");

        scoreBox.gameObject.SetActive(true);
        explosion.gameObject.SetActive(false);
        score.gameObject.SetActive(true);
    }

}
