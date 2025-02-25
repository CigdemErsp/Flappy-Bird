using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text countdownText;
    [SerializeField] private TMP_Text superPowerCountdown;

    [SerializeField] private GameObject playButton;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private GameObject getReady;

    [SerializeField] private Player player;   
    [SerializeField] private PipeSpawner pipeSpawner;

    [SerializeField] private CheckpointData checkpointData;

    private ScoreManager scoreManager;
    private LeaderboardManager leaderboardManager;

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();
        leaderboardManager = FindObjectOfType<LeaderboardManager>();

        player.enabled = false;
        gameOver.SetActive(false);
        Application.targetFrameRate = 60;   
        pipeSpawner.enabled = false;
        player.OnCountdownUpdated += UpdateCountdownText;
    }

    public void OnClick()
    {
        FindObjectOfType<CheckpointManager>().LoadCheckpoint(player.gameObject);
        FindObjectOfType<DistanceTracker>().UpdateDistanceToCheckpoint(checkpointData.Distance);
        scoreManager.UpdateScoreManagerToCheckpoint(checkpointData.PlayerScore, checkpointData.PlayerCoin);
        leaderboardManager.enabled = false;
        scoreManager.ScoreText.gameObject.SetActive(false);
        pipeSpawner.enabled = false;
        getReady.SetActive(true);

        countdownText.gameObject.SetActive(true);
        player.enabled = false;
        StartCoroutine(OnStart());
    }

    public IEnumerator OnStart()
    {
        FindObjectOfType<Pipes>().Score = scoreManager.Score;
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
        FindObjectOfType<DistanceTracker>().enabled = true;
        pipeSpawner.enabled = true;
        getReady.SetActive(false);
        scoreManager.ScoreText.gameObject.SetActive(true);

        player.enabled = true;
    }

    public void Pause()
    {
        FindObjectOfType<DistanceTracker>().enabled = false;
        Time.timeScale = 0f;
        player.enabled = true;
    }

    public void GameOver()
    {
        scoreManager.CheckHighscore();
        gameOver.SetActive(true);
        playButton.SetActive(true);
        superPowerCountdown.gameObject.SetActive(false);
        player.StopAllCoroutines();
        player.superPowerActivated = false;

        leaderboardManager.enabled = true;

        Pause();
    }

    private void UpdateCountdownText(int countdown)
    {
        if (countdown >= 0)
        {
            superPowerCountdown.text = countdown.ToString();
            if (!superPowerCountdown.IsActive())
            {
                superPowerCountdown.gameObject.SetActive(true);
            }
        }
        else
        {
            superPowerCountdown.gameObject.SetActive(false);
        }
    }

    public void GameEnd() 
    {
        FindObjectOfType<DistanceTracker>().enabled = false;
        scoreManager.CheckHighscore();
        superPowerCountdown.gameObject.SetActive(false);
        player.StopAllCoroutines();
        player.superPowerActivated = false;
        player.StartSmoothResetPos();

        leaderboardManager.enabled = true;
        scoreManager.ScoreText.gameObject.SetActive(false);
        pipeSpawner.enabled = false;

        player.enabled = false;
    }

    void OnEnable()
    {
        Time.timeScale = 0f;
        player.OnGameEnd += GameEnd;
    }

}
