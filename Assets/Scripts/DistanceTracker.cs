using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class DistanceTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private Slider distanceTracker;
    [SerializeField] private Animator backgroundAnimator;

    private float distanceTravelled = 0f;
    private float lengthOfTracker;
    private float animationSpeed;

    private PipeSpawner pipeSpawner;

    public float DistanceTravelled { get { return distanceTravelled; } set { distanceTravelled = value; } }

    private void Awake()
    {
        animationSpeed = backgroundAnimator.speed;
        pipeSpawner = FindObjectOfType<PipeSpawner>();
        lengthOfTracker = pipeSpawner.DistanceNeededToWin;
        distanceTracker.maxValue = lengthOfTracker + pipeSpawner.SpawnThreshold;
        distanceTracker.value = 0f;

    }

    private void Update()
    {
        distanceTravelled += animationSpeed * Time.deltaTime;
        distanceText.text = ((int)distanceTravelled).ToString();
        distanceTracker.value = distanceTravelled;
    }

    public float GetDistanceTraveled()
    {
        return distanceTravelled;
    }

    public void ResetDistance() {  
        distanceTravelled = 0f;
        distanceText.text = ((int)distanceTravelled).ToString();
        distanceTracker.value = 0f;
    }

    public void UpdateDistanceToCheckpoint(float checkpointDistance)
    {
        distanceTravelled = checkpointDistance;
        distanceText.text = ((int)distanceTravelled).ToString();
        distanceTracker.value = distanceTravelled;
    }

}