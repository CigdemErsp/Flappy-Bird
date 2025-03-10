using TMPro;
using UnityEngine;
using DG.Tweening;
using System.Collections;
using System;
using System.Collections.Generic;

public class PopUpManager : MonoBehaviour
{
    #region serializefields
    [SerializeField] private DistanceTracker _distanceTracker;
    [SerializeField] private EffectDatabase _effectDatabase;

    [SerializeField] private CanvasGroup _gameCanvas;
    [SerializeField] private Canvas _popUpCanvas;

    [SerializeField] private CanvasGroup _popUpOptionLeft;
    [SerializeField] private TMP_Text _leftPopUpTitle;
    [SerializeField] private TMP_Text _leftPopUpDescription;

    [SerializeField] private CanvasGroup _popUpOptionRight;
    [SerializeField] private TMP_Text _rightPopUpTitle;
    [SerializeField] private TMP_Text _rightPopUpDescription;
    #endregion

    #region private fields
    private float _fadeDuration = 1.0f;
    private List<RoguelikeEffect> _availableEffects;

    private RoguelikeEffect _chosenEffectLeft;
    private RoguelikeEffect _chosenEffectRight;
    #endregion

    private void Awake()
    {
        _availableEffects = new List<RoguelikeEffect>(_effectDatabase.Effects);
    }

    private void ReadyPopUps()
    {
        int randIndex = UnityEngine.Random.Range(0, _availableEffects.Count);
        _chosenEffectLeft = _availableEffects[randIndex];
        _availableEffects.RemoveAt(randIndex);

        randIndex = UnityEngine.Random.Range(0, _availableEffects.Count);
        _chosenEffectRight = _availableEffects[randIndex];

        _leftPopUpTitle.text = _chosenEffectLeft.EffectName;
        _leftPopUpDescription.text = _chosenEffectLeft.Description;

        _rightPopUpTitle.text = _chosenEffectRight.EffectName;
        _rightPopUpDescription.text = _chosenEffectRight.Description;

        _availableEffects.Add(_chosenEffectLeft);
    }

    private void OnEnablePopUp()
    {
        ReadyPopUps();
        _popUpCanvas.gameObject.SetActive(true);
        StartCoroutine(EnablePopUp());
    }

    private IEnumerator EnablePopUp()
    {  
        _gameCanvas.DOFade(0.5f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);

        _popUpOptionLeft.DOFade(1f, _fadeDuration);
        _popUpOptionRight.DOFade(1f, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
    }

    public void OnLeftButtonClick()
    {
        // When left button is clicked, simply call the effect's OnClick.
        if (_chosenEffectLeft != null)
        {
            _chosenEffectLeft.OnClick();
        }
    }

    public void OnRightButtonClick()
    {
        // When right button is clicked, simply call the effect's OnClick.
        if (_chosenEffectRight != null)
        {
            _chosenEffectRight.OnClick();
        }
    }

    private void OnEnable()
    {
        _distanceTracker.OnDistanceThresholdReached += OnEnablePopUp;
    }

    private void OnDisable()
    {
        _distanceTracker.OnDistanceThresholdReached -= OnEnablePopUp;
    }
}
