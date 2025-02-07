using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private List<(int score, int coins)> scores = new List<(int, int)>(); // Store both score and coins
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private TextMeshProUGUI[] coinTexts;
    [SerializeField] private GameObject leaderBoard;

    private void Start()
    {
        UpdateLeaderboardUI();
    }

    public void AddScore(int newScore, int newCoins)
    {
        if (scores.Count < 5 || newScore > scores[^1].score)
        {
            scores.Add((newScore, newCoins));
            scores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort by score (descending)

            if (scores.Count > 5)
            {
                scores.RemoveAt(5);
            }

            SaveScores();
        }
    }

    public List<(int, int)> GetScores()
    {
        return scores;
    }

    private void SaveScores()
    {
        PlayerPrefs.SetInt("HighscoreCount", scores.Count);

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"Score{i}", scores[i].score);
            PlayerPrefs.SetInt($"Coins{i}", scores[i].coins);
        }

        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        scores.Clear();
        int scoreCount = PlayerPrefs.GetInt("HighscoreCount", 0);

        for (int i = 0; i < scoreCount; i++)
        {
            int score = PlayerPrefs.GetInt($"Score{i}", 0);
            int coins = PlayerPrefs.GetInt($"Coins{i}", 0);
            scores.Add((score, coins));
        }
    }

    public void UpdateLeaderboardUI()
    {
        LoadScores();

        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < scores.Count)
            {
                scoreTexts[i].text = scores[i].score.ToString();
                coinTexts[i].text = scores[i].coins.ToString();
            }
            else
            {
                scoreTexts[i].text = "";
                coinTexts[i].text = "";
            }
        }
    }

    private void OnEnable()
    {
        UpdateLeaderboardUI();
        leaderBoard.gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        if (!gameObject.activeInHierarchy) return;
        leaderBoard.gameObject.SetActive(false);
    }
}