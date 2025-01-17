using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject getReady;

    [SerializeField] private Player player;   
    [SerializeField] private PipeSpawner pipeSpawner;

    private ScoreManager scoreManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        player.enabled = false;
        gameOver.SetActive(false);
        Application.targetFrameRate = 60;   
        pipeSpawner.enabled = false;
    }

    public void OnClick()
    {
        scoreManager.getScoreText().gameObject.SetActive(false);
        pipeSpawner.enabled = false;
        getReady.SetActive(true);

        Pipes[] pipes = FindObjectsOfType<Pipes>();

        for (int i = 0; i < pipes.Length; i++)
        {
            Destroy(pipes[i].gameObject);
        }

        countdownText.gameObject.SetActive(true);
        player.enabled = false;
        StartCoroutine(OnStart());
    }

    public IEnumerator OnStart()
    {
        scoreManager.setScore(0);
        float countdown = 3f;
        playButton.SetActive(false);
        gameOver.SetActive(false);

        Time.timeScale = 1f;

        while(countdown > 0)
        {
            countdownText.text = countdown.ToString();

            // Wait for 1 second
            yield return new WaitForSeconds(1);

            // Decrement the countdown
            countdown--;
        }
        countdownText.gameObject.SetActive(false);
        Play();
    }

    public void Play()
    {
        pipeSpawner.enabled = true;
        getReady.SetActive(false);
        scoreManager.getScoreText().gameObject.SetActive(true);
        scoreManager.setScore(0);

        player.enabled = true;
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        player.enabled = true;
    }

    public void GameOver()
    {
        gameOver.SetActive(true);
        playButton.SetActive(true);

        Pause();
    }

    void OnEnable()
    {
        Time.timeScale = 0f;
    }

}
