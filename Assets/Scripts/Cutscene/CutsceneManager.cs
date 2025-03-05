using UnityEngine;
using TMPro;
using System.Collections;
using DG.Tweening;
using System.Collections.Generic;

public class CutsceneManager : MonoBehaviour
{
    #region serializefields
    [SerializeField] private StartCutscene _startCutscene;
    [SerializeField] private EndingCutscene _endingCutscene;
    #endregion

    private void OnGameStart()
    {
        _startCutscene.OnGameStartCutscene();
    }

    private void OnGameEnd()
    {
        _endingCutscene.OnGameEndCutscene();
    }

    private void OnEnable()
    {
        GameManager.Instance.OnStartGame += OnGameStart;
        GameManager.Instance.OnEndGame += OnGameEnd;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnStartGame -= OnGameStart;
        GameManager.Instance.OnEndGame -= OnGameEnd;
    }
}