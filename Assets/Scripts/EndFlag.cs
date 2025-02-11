using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndFlag : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;
    }
}
