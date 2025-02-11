using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameObject superPower;

    private Vector3 direction;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float strength = 5f;

    public bool superPowerActivated = false;

    private ScoreManager scoreManager;

    public event Action<int> OnCountdownUpdated;
    public event Action OnGameEnd;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            direction = Vector3.up * strength;
        }

        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {  // beginning to touch
                direction = Vector3.up * strength;
            }
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        float angle = Mathf.Clamp(direction.y * 5f, -45f, 45f); // Adjust sensitivity if needed
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            if(!superPowerActivated)
                FindObjectOfType<GameManager>().GameOver();
            else
                scoreManager.IncreaseScore();
        }           
        else if(collision.gameObject.tag == "Ground")
        {
            FindObjectOfType<GameManager>().GameOver();
        }
        else if (collision.gameObject.tag == "Coin")
        {
            collision.gameObject.transform.parent.gameObject.transform.Find("Explosion").gameObject.SetActive(true);
            collision.gameObject.transform.parent.gameObject.transform.Find("Score Box").gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
            scoreManager.IncreaseCoinCount();
            scoreManager.IncreaseScore();
        }
        else if (collision.gameObject.CompareTag("Score"))
        {
            scoreManager.IncreaseScore();
        }
        else if(collision.gameObject.CompareTag("heart"))
        {
            collision.gameObject.transform.parent.gameObject.transform.Find("Explosion").gameObject.SetActive(true);
            collision.gameObject.SetActive(false);
            superPowerActivated = true;
            superPower.SetActive(true);
            StartCoroutine(ActivateSuperPower());
        }
        else if(collision.gameObject.CompareTag("GameEnd"))
        {
            collision.gameObject.transform.Find("Explosion").gameObject.SetActive(true);
            OnGameEnd?.Invoke();
        }
    }

    private IEnumerator ActivateSuperPower()
    {
        float countdown = 10f;

        Time.timeScale = 1f;

        while (countdown >= 0)
        {
            OnCountdownUpdated?.Invoke(((int)countdown));
            // Wait for 1 second
            yield return new WaitForSeconds(1);

            // Decrement the countdown
            countdown--;
        }
        OnCountdownUpdated?.Invoke(-1);
        superPowerActivated = false;
        superPower.SetActive(false);
    }

    private void OnEnable()
    {
        ResetPos();
    }

    public void OnDisable()
    {
        superPower.gameObject.SetActive(false);
        ResetPos();
    }

    private void ResetPos()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
        transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    private IEnumerator SmoothResetPos()
    {
        Vector3 startPosition = transform.position;
        Vector3 targetPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(0, 0, 0);

        float elapsed = 0f;

        while (elapsed < 1f)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / 1f; // Normalize to 0-1 range

            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            yield return null;
        }

        transform.position = targetPosition;
        transform.rotation = targetRotation;
    }

    public void StartSmoothResetPos()
    {
        StartCoroutine(SmoothResetPos());
    }

}
