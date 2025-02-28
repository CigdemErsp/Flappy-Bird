using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LeaderboardManager : MonoBehaviour
{
    #region serializefields
    [SerializeField] private TextMeshProUGUI[] _scoreTexts;
    [SerializeField] private TextMeshProUGUI[] _coinTexts;
    [SerializeField] private GameObject _leaderBoard;
    #endregion

    #region private fields
    private List<(int score, int coins)> _scores = new List<(int, int)>(); // Store both score and coins
    #endregion

    public List<(int score, int coins)> Scores 
    { 
        get 
        { 
            return _scores; 
        } 
    }

    private void Start()
    {
        UpdateLeaderboardUI();
    }

    public void UpdateLeaderboardUI()
    {
        LoadScores();

        for (int i = 0; i < _scoreTexts.Length; i++)
        {
            if (i < _scores.Count)
            {
                _scoreTexts[i].text = _scores[i].score.ToString();
                _coinTexts[i].text = _scores[i].coins.ToString();
            }
            else
            {
                _scoreTexts[i].text = "";
                _coinTexts[i].text = "";
            }
        }
    }

    public void AddScore(int newScore, int newCoins)
    {
        if (CheckScoreToAddBoard(newScore))
        {
            _scores.Add((newScore, newCoins));
            _scores.Sort((a, b) => b.score.CompareTo(a.score)); // Sort by score (descending)

            if (_scores.Count > 5)
            {
                _scores.RemoveAt(5);
            }

            SaveScores();
        }
    }

    private void SaveScores()
    {
        PlayerPrefs.SetInt("HighscoreCount", _scores.Count);

        for (int i = 0; i < _scores.Count; i++)
        {
            PlayerPrefs.SetInt($"Score{i}", _scores[i].score);
            PlayerPrefs.SetInt($"Coins{i}", _scores[i].coins);
        }

        PlayerPrefs.Save();
    }

    private void LoadScores()
    {
        _scores.Clear();
        int scoreCount = PlayerPrefs.GetInt("HighscoreCount", 0);

        for (int i = 0; i < scoreCount; i++)
        {
            int score = PlayerPrefs.GetInt($"Score{i}", 0);
            int coins = PlayerPrefs.GetInt($"Coins{i}", 0);
            _scores.Add((score, coins));
        }
    }

    private void OnEnable()
    {
        UpdateLeaderboardUI();
        _leaderBoard.gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        if (!gameObject.activeInHierarchy) return;
        _leaderBoard.gameObject.SetActive(false);
    }

    private bool CheckScoreToAddBoard(int newScore)
    {
        return _scores.Count < 5 || newScore > _scores[^1].score;
    }
       
}