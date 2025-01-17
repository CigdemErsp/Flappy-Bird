using TMPro;
using UnityEngine;
using static GameManager;

public class ScoreManager : MonoBehaviour
{
    private int score = 0;
    private int coinCount;
    [SerializeField] private TMP_Text coinCountText;
    [SerializeField] private TMP_Text scoreText;

    public void setScore(int score) { 
        this.score = score;
        scoreText.text = score.ToString();
    }
    public int getScore() { return score; }

    public void setCoinCount(int coinCount) { 
        this.coinCount = coinCount;
        coinCountText.text = "x " + coinCount.ToString();
    }
    public int getCoinCount() { return coinCount; }

    public TMP_Text getScoreText() { return scoreText; }

    public TMP_Text getCoinCountText() { return coinCountText; }

    private void Start()
    {
        coinCount = 0;
    }

    public void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    public void IncreaseCoinCount() { 
        coinCount++;
        coinCountText.text = "x " + coinCount.ToString();
    }
}
