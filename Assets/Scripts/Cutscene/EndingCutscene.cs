using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class EndingCutscene : MonoBehaviour
{
    #region actions
    public event Action OnCutsceneEnd;
    #endregion

    #region serializeFields
    [SerializeField] private Transform _player;
    [SerializeField] private Transform _tree;

    [SerializeField] private TMP_Text _playerDialogueText;
    [SerializeField] private TMP_Text _treeDialogueText;
    [SerializeField] private TMP_Text _endText;

    [SerializeField] private GameObject _playerDialogueBubble;
    [SerializeField] private GameObject _treeDialogueBubble;

    [SerializeField] private Animator _backgroundAnimator;
    [SerializeField] private Animator _groundAnimator;
    #endregion

    #region private fields
    private Vector3 _targetPlayerPosition = new(-6.65999985f, -1.40999997f, -0.123170152f);
    private Vector3 _targetTreePosition = new(6.34000015f, -3.50999999f, 0);
    private Vector3 _targetPlayerEndingPosition = new(-15, -1.40999997f, -0.123170152f);
    private Vector3 _targetTreeEndingPosition = new(-15, -3.50999999f, 0);
    private float moveDuration = 2f;
    private float fadeDuration = 1f;
    private float _typingSpeed = 0.05f;
    #endregion

    public void OnGameEndCutscene()
    {
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator PlayCutscene()
    {
        DeactivatePlayer();
        _player.DOMove(_targetPlayerPosition, moveDuration).SetEase(Ease.InOutSine);
        _tree.DOMove(_targetTreePosition, moveDuration).SetEase(Ease.InOutSine);
        yield return new WaitForSeconds(moveDuration);

        DOTween.To(() => _backgroundAnimator.speed, x => _backgroundAnimator.speed = x, 0f, 1f);
        DOTween.To(() => _groundAnimator.speed, x => _groundAnimator.speed = x, 0f, 1f);

        //yield return FadeInDialogue();

        // Start the dialogue
        yield return ShowDialogueBubble(_treeDialogueBubble);
        yield return ShowDialogue("You have done the impossible... welcome to the Sky Temple.", _treeDialogueText);
        yield return ShowDialogueBubble(_playerDialogueBubble);
        yield return ShowDialogue("I... I made it?", _playerDialogueText);
        yield return ShowDialogue("Your wings have carried you beyond limits. You are free now, little one.", _treeDialogueText);
        yield return HideDialogueBubble(_playerDialogueBubble, _playerDialogueText);
        yield return HideDialogueBubble(_treeDialogueBubble, _treeDialogueText);
        
        //yield return FadeOutDialogueAndStartGame();

        _player.DOMove(_targetPlayerEndingPosition, moveDuration).SetEase(Ease.InOutSine);
        _tree.DOMove(_targetTreeEndingPosition, moveDuration).SetEase(Ease.InOutSine);

        yield return ShowDialogue("THE END – You have become a legend.", _endText);

        OnCutsceneEnd?.Invoke();
    }

    private void DeactivatePlayer()
    {
        _player.GetComponent<Rigidbody2D>().simulated = false;
        _player.GetComponent<CircleCollider2D>().enabled = false;
        _player.GetComponent<Player>().enabled = false;
        _player.GetComponent<PlayerController>().enabled = false;
    }

    private IEnumerator ShowDialogueBubble(GameObject _dialogueBubble)
    {
        _dialogueBubble.GetComponent<SpriteRenderer>().DOFade(1, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator HideDialogueBubble(GameObject _dialogueBubble, TMP_Text _dialogueText)
    {
        _dialogueText.text = "";
        _dialogueBubble.GetComponent<SpriteRenderer>().DOFade(0, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
    }

    private IEnumerator ShowDialogue(string text, TMP_Text _dialogueText)
    {
        _dialogueText.text = "";
        yield return new WaitForSeconds(fadeDuration);

        // Use coroutine to display text letter by letter
        foreach (char letter in text)
        {
            _dialogueText.text += letter; // Append each letter
            yield return new WaitForSeconds(_typingSpeed); // Wait between letters
        }

        // Wait for a moment before moving to the next dialogue
        yield return new WaitForSeconds(1f);
    }

    //private IEnumerator FadeOutDialogueAndStartGame()
    //{
    //    // Fade out the dialogue canvas
    //    _treeDialogueBubble.GetComponent<SpriteRenderer>().DOFade(0, fadeDuration);
    //    _playerDialogueBubble.GetComponent<SpriteRenderer>().DOFade(0, fadeDuration);
    //    yield return new WaitForSeconds(fadeDuration); // Wait for the fade-out to complete

    //    DeactivateDialoguePanels();

    //    // Fade in the game canvas
    //    _gameCanvas.gameObject.SetActive(true);
    //    _gameCanvas.DOFade(1, fadeDuration);
    //}

    //private IEnumerator FadeInDialogue()
    //{
    //    _gameCanvas.DOFade(0, fadeDuration);
    //    yield return new WaitForSeconds(fadeDuration);
    //    _gameCanvas.gameObject.SetActive(false);

    //    _dialogueCanvas.gameObject.SetActive(true);
    //    _dialogueCanvas.DOFade(1, fadeDuration);
    //}
}
