using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreManager : MonoBehaviour
{
    #region static fields
    private static int _score = 0;
    private static int _coinCount = 0;
    #endregion

    #region serializefields
    [SerializeField] private LeaderboardManager _leaderboardManager;
    [SerializeField] private TMP_Text _coinCountText;
    [SerializeField] private TMP_Text _scoreText;
    [SerializeField] private TMP_Text _highscoreText;
    [SerializeField] private PopUpManager _popUpManager;
    #endregion

    #region private fields
    private int _coinMultiplier;
    #endregion

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            _scoreText.text = value.ToString();
        }
    }

    public int CoinCount
    {
        get { return _coinCount; }
        set
        {
            _coinCount = value;
            _coinCountText.text = "x " + value.ToString();
        }
    }

    public TMP_Text ScoreText
    {
        get { return _scoreText; }
    }

    public TMP_Text CoinCountText
    {
        get { return _coinCountText; }
    }

    private void Start()
    {
        _highscoreText.text = "Highscore: " + PlayerPrefs.GetInt("Highscore");
        _coinMultiplier = 1;
    }

    public void IncreaseScore()
    {
        _score += 1;
        _scoreText.text = _score.ToString();
    }

    public void IncreaseCoinCount()
    {
        _coinCount += _coinMultiplier;
        _coinCountText.text = "x " + _coinCount.ToString();
    }

    public void CheckHighscore()
    {
        if (PlayerPrefs.GetInt("Highscore") < _score)
        {
            PlayerPrefs.SetInt("Highscore", _score);
            _highscoreText.text = "Highscore: " + _score.ToString();
        }
        _leaderboardManager.AddScore(_score, _coinCount);
    }

    // Update score and coin count with the checkpoint values
    public void UpdateScoreManagerToCheckpoint(int score, int coin)
    {
        _score = score;
        _scoreText.text = ((int)score).ToString();

        _coinCount = coin;
        _coinCountText.text = "x " + coin.ToString();
    }

    private void ApplyEffect(RoguelikeEffect roguelikeEffect)
    {
        // in case more effects will be added
        switch(roguelikeEffect.EffectName)
        {
            case "Gold Master":
                _coinMultiplier *= 2;
                break;
        }
    }

    private void ResetEffects()
    {
        _coinMultiplier = 1;
    }

    private void OnEnable()
    {
        _popUpManager.OnEffectSelected += ApplyEffect;
        GameManager.Instance.OnGameOver += ResetEffects;
    }

    private void OnDisable()
    {
        _popUpManager.OnEffectSelected -= ApplyEffect;
    }
}