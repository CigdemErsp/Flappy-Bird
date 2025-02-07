using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DistanceTracker : MonoBehaviour
{
    [SerializeField] private TMP_Text distanceText;
    [SerializeField] private GameObject playerAvatar;

    private float distanceTraveled = 0f;


    private void Update()
    {
        distanceTraveled += FindObjectOfType<Background>().AnimationSpeed * Time.deltaTime;
        playerAvatar.transform.position = new Vector3(playerAvatar.transform.position.x + 0.0007f, playerAvatar.transform.position.y, playerAvatar.transform.position.z);

        distanceText.text = ((int)distanceTraveled).ToString();
    }

    public float GetDistanceTraveled()
    {
        return distanceTraveled;
    }

    public void ResetDistance() {  
        distanceTraveled = 0f;
        distanceText.text = ((int)distanceTraveled).ToString();
    }

}