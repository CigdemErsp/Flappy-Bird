using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    private List<int> scores = new List<int>();
    [SerializeField] private TextMeshProUGUI[] scoreTexts;
    [SerializeField] private GameObject leaderBoard;

    private void Start()
    {
        UpdateLeaderboardUI();
    }

    public void AddScore(int newScore)
    { 
        if (scores.Count < 5 || newScore > scores[^1])
        {
            scores.Add(newScore);
            scores.Sort((a, b) => b.CompareTo(a)); 

            if (scores.Count > 5)
            {
                scores.RemoveAt(5);
            }

            SaveScores();
        }
    }

    public List<int> GetScores()
    {
        return scores;
    }

    private void SaveScores()
    {
        for (int i = 0; i < scores.Count; i++)
        {
            PlayerPrefs.SetInt($"Score{i}", scores[i]);
        }

        PlayerPrefs.SetInt("HighscoreCount", scores.Count);
        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        scores.Clear();
        int scoreCount = PlayerPrefs.GetInt("HighscoreCount", 0);

        for (int i = 0; i < scoreCount; i++)
        {
            scores.Add(PlayerPrefs.GetInt($"Score{i}", 0));
        }
    }

    public void UpdateLeaderboardUI()
    {
        LoadScores();
        for (int i = 0; i < scoreTexts.Length; i++)
        {
            if (i < scores.Count)
            {
                scoreTexts[i].text = scores[i].ToString();
            }
            else
            {
                scoreTexts[i].text = "";
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