using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartCutscene : MonoBehaviour
{
    #region actions
    public event Action OnCutsceneEnd;
    #endregion

    #region SerializeFields
    [SerializeField] private List<TMP_Text> _dialogueTexts;  // TextMeshPro text object to display the dialogue
    [SerializeField] private List<CanvasGroup> _dialogueCanvasGroups;  
    [SerializeField] private GameObject _dialoguePanel; // Panel that holds the dialogue box
    
    [SerializeField] private CanvasGroup _gameCanvas; 
    [SerializeField] private CanvasGroup _dialogueCanvas;

    [SerializeField] private CheckpointData _checkpointData;
    #endregion

    #region private fields
    private float _typingSpeed = 0.05f; // Speed of typing effect
    private float _fadeDuration = 1f;
    #endregion

    public void OnGameStartCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        //if (!_checkpointData.HasCheckpoint)
        //{
        //    yield return FadeInDialogue();

        //    // Start the dialogue
        //    yield return ShowDialogue("Owl: 'Welcome to the Sky Temple, little bird.'", _dialogueTexts[0], _dialogueCanvasGroups[0]);
        //    yield return ShowDialogue("Flappy Bird: 'I’ve traveled far to reach this place...'", _dialogueTexts[1], _dialogueCanvasGroups[1]);
        //    yield return ShowDialogue("Owl: 'The trials ahead will test your courage.'", _dialogueTexts[2], _dialogueCanvasGroups[2]);
        //    yield return ShowDialogue("Flappy Bird: 'I’m ready! Let me face the challenges!'", _dialogueTexts[3], _dialogueCanvasGroups[3]);

        //    yield return FadeOutDialogueAndStartGame();
        //}
        yield return new WaitForEndOfFrame();
        OnCutsceneEnd?.Invoke();
    }

    private IEnumerator ShowDialogue(string text, TMP_Text _dialogueText, CanvasGroup _dialogueCanvasGroup)
    {
        _dialogueText.text = "";
        _dialoguePanel.SetActive(true);
        _dialogueCanvasGroup.DOFade(1, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        // Clear previous dialogue and show the panel
        

        // Use coroutine to display text letter by letter
        foreach (char letter in text)
        {
            _dialogueText.text += letter; // Append each letter
            yield return new WaitForSeconds(_typingSpeed); // Wait between letters
        }

        // Wait for a moment before moving to the next dialogue
        yield return new WaitForSeconds(1f);
    }

    private IEnumerator FadeOutDialogueAndStartGame()
    {
        // Fade out the dialogue canvas
        _dialogueCanvas.DOFade(0, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration); // Wait for the fade-out to complete
        _dialogueCanvas.gameObject.SetActive(false); // Deactivate after fade-out

        DeactivateDialoguePanels();

        // Fade in the game canvas
        _gameCanvas.gameObject.SetActive(true);
        _gameCanvas.DOFade(1, _fadeDuration);
    }

    private IEnumerator FadeInDialogue()
    {
        _gameCanvas.DOFade(0, _fadeDuration);   
        yield return new WaitForSeconds(_fadeDuration);
        _gameCanvas.gameObject.SetActive(false);

        _dialogueCanvas.gameObject.SetActive(true);
        _dialogueCanvas.DOFade(1, _fadeDuration); 
    }

    private void DeactivateDialoguePanels()
    {
        foreach(var _dialogueCanvasGroup in _dialogueCanvasGroups)
        {
            _dialogueCanvasGroup.alpha = 0;
        }
    }
}
