using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static GameManager;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int coinCount;

    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text highscoreText;

    private LeaderboardManager leaderboardManager;

    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = value.ToString();
        }
    }

    public int CoinCount { 
        get { return coinCount; } 
        set
        { 
            coinCount = value; 
            coinCountText.text = "x " + value.ToString();
        } 
    }

    public TMP_Text ScoreText
    {
        get { return scoreText; }
    }

    public TMP_Text CoinCountText { 
        get { return coinCountText; } }

    private void Start()
    {
        coinCount = 0;
        highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
        leaderboardManager = FindObjectOfType<LeaderboardManager>();
    }

    public void IncreaseScore()
    {
        score += 1;
        scoreText.text = score.ToString();
    }

    public void IncreaseCoinCount() { 
        coinCount += 1;
        coinCountText.text = "x " + coinCount.ToString();
    }

    public void CheckHighscore()
    {
        if (PlayerPrefs.GetInt("Highscore") < score)
        {
            PlayerPrefs.SetInt("Highscore", score);
            highscoreText.text = "Highscore: " + score.ToString();
        }
        leaderboardManager.AddScore(score);
    }
}
