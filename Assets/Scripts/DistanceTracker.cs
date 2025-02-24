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

    private float distanceTravelled = 0f;
    private float lengthOfTracker;

    private Background background;
    private PipeSpawner pipeSpawner;

    public float DistanceTravelled { get { return distanceTravelled; } }

    private void Awake()
    {
        background = FindObjectOfType<Background>();
        pipeSpawner = FindObjectOfType<PipeSpawner>();
        lengthOfTracker = pipeSpawner.DistanceNeededToWin;
        distanceTracker.maxValue = lengthOfTracker + pipeSpawner.SpawnThreshold;
        distanceTracker.value = 0f;

    }

    private void Update()
    {
        distanceTravelled += background.AnimationSpeed * Time.deltaTime;
        distanceText.text = ((int)distanceTravelled).ToString();
        distanceTracker.value = distanceTravelled;
        Debug.Log(distanceTravelled);
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
}