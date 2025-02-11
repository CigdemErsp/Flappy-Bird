using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static Unity.IO.LowLevel.Unsafe.AsyncReadManagerMetrics;

public class DistanceTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private GameObject playerAvatar;
    [SerializeField] private GameObject positionAnchor;
    [SerializeField] private GameObject flag;

    private float distanceTravelled = 0f;
    private float newDistance = 0f;
    private float speed;
    private float lengthOfTracker;
    private float distanceNeededToWin;

    private Background background;
    private PipeSpawner pipeSpawner;

    private void LengthOfTracker()
    {
        lengthOfTracker = Vector3.Distance(positionAnchor.transform.position, flag.transform.position); // distance of the tracker to calculate the movement of playerAvatar
    }

    private void Awake()
    {
        background = FindObjectOfType<Background>();
        pipeSpawner = FindObjectOfType<PipeSpawner>();
        LengthOfTracker();
    }

    private void Update()
    {
        newDistance = background.AnimationSpeed * Time.deltaTime;
        distanceTravelled += newDistance; // incase of speed change

        distanceNeededToWin = pipeSpawner.DistanceNeededToWin + pipeSpawner.SpawnThreshold; 

        speed = lengthOfTracker * newDistance / distanceNeededToWin;

        Debug.Log(speed);

        playerAvatar.transform.position = new Vector3(playerAvatar.transform.position.x + speed, playerAvatar.transform.position.y, playerAvatar.transform.position.z);

        distanceText.text = ((int)distanceTravelled).ToString();
    }

    public float GetDistanceTraveled()
    {
        return distanceTravelled;
    }

    public void ResetDistance() {  
        distanceTravelled = 0f;
        playerAvatar.transform.localPosition = positionAnchor.transform.localPosition;
        distanceText.text = ((int)distanceTravelled).ToString();
    }

}