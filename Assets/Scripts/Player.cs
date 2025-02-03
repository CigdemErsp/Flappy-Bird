using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public Sprite[] sprites;
    private int spriteIndex;

    [SerializeField] private GameObject superPower;

    private Vector3 direction;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float strength = 5f;

    public bool superPowerActivated = false;

    private ScoreManager scoreManager;

    public event Action<int> OnCountdownUpdated;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        scoreManager = FindObjectOfType<ScoreManager>();
    }

    private void Start()
    {
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
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

            if(touch.phase == TouchPhase.Began) // beginning to touch
                direction = Vector3.up * strength;
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        spriteRenderer.sprite = sprites[spriteIndex];
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
        else if (collision.gameObject.tag == "Score")
        {
            scoreManager.IncreaseScore();
        }
        else if(collision.gameObject.tag == "heart")
        {
            collision.gameObject.transform.parent.gameObject.transform.Find("Explosion").gameObject.SetActive(true);
            collision.gameObject.SetActive(false);
            superPowerActivated = true;
            superPower.gameObject.SetActive(true);
            StartCoroutine(ActivateSuperPower());
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
        superPower.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        ResetPos();
    }

    public void OnDisable()
    {
        superPower.gameObject.SetActive(false);
        ResetPos();
        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
    }

    private void ResetPos()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }
}
