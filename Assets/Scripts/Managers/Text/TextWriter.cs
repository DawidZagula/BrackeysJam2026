using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    public static TextWriter Instance { get; private set; }

    public enum TextFieldsId
    {
        TopLeft,
        TopMiddle,
        BottomLeft,
        BottomMiddle
    }

    [Header("Configuration")]
    [SerializeField] private bool _hasSingleTextField;

    [Header("References")]
    [SerializeField] private TextMeshProUGUI[] _levelTextFields;
    [SerializeField] private TextMeshProUGUI _currentTextField;
    [SerializeField] private LevelText _levelText;

    private Dictionary<TextFieldsId, TextMeshProUGUI> _idTextFieldMap
        = new Dictionary<TextFieldsId, TextMeshProUGUI>();

    //Run-time state
    private Coroutine _currentTypingRoutine;

    private void Awake()
    {
        Instance = this;

        if (_hasSingleTextField)
        {
            _currentTextField.gameObject.SetActive(false);
        }
        else
        {
            InitializeIdTextFieldMap();
            DisableTextFields();
        }
    }

    private void InitializeIdTextFieldMap()
    {
        _idTextFieldMap[TextFieldsId.TopLeft] = _levelTextFields[0];
        _idTextFieldMap[TextFieldsId.TopMiddle] = _levelTextFields[1];
        _idTextFieldMap[TextFieldsId.BottomLeft] = _levelTextFields[1];
        _idTextFieldMap[TextFieldsId.BottomMiddle] = _levelTextFields[1];
    }

    private void DisableTextFields()
    {
        foreach (TextMeshProUGUI textField in _levelTextFields)
        {
            textField.gameObject.SetActive(false);
        }
    }

    public void StartTypingSequence(TextSequenceId sequenceId, bool shouldClearAtEnd, Action onFinish = null)
    {
        ToggleCurrentTextFieldVisilibity(true);

        foreach (TextSequence textSequence in _levelText.TextSequences)
        {
            if (textSequence.TextSequenceId == sequenceId)
            {
                if (_currentTypingRoutine != null) return;
                _currentTypingRoutine = StartCoroutine(TypingRoutine(textSequence, shouldClearAtEnd, onFinish));
                return;
            }
        }
    }

    private IEnumerator TypingRoutine(TextSequence textSequence, bool shouldClearAtEnd, Action onFinish)
    {
        int textSegmentsCount = textSequence.TextSegments.Length;
        int currentSegmentIndex = 0;
        while (currentSegmentIndex < textSegmentsCount)
        {
            _currentTextField.text = string.Empty;

            TextSegment currentSegment = textSequence.TextSegments[currentSegmentIndex];
            float timeToNextSegment = currentSegment.TimeToNextSegment;
            float timeBetweenLetters = currentSegment.TimeBetweenLetters;

            int currentLetterIndex = 0;

            while (currentLetterIndex < currentSegment.Text.Length)
            {
                yield return new WaitForSeconds(timeBetweenLetters);

                char letterToPrint = currentSegment.Text[currentLetterIndex];
                _currentTextField.text += letterToPrint;

                currentLetterIndex++;
            }
            currentSegmentIndex++;
            if (currentSegmentIndex < textSegmentsCount)
            {
                yield return new WaitForSeconds(timeToNextSegment);
            }
        }

        float timeToHideLastSegment = textSequence.TimeToHideLastSegment;
        yield return new WaitForSeconds(timeToHideLastSegment);

        if (shouldClearAtEnd)
        {
            ClearText();
        }
        _currentTypingRoutine = null;

        if (onFinish != null)
        {
            onFinish();
        }
    }

    public void SetTextField(TextFieldsId textFieldId)
    {
        _currentTextField = _idTextFieldMap[textFieldId];
    }

    public void ClearText()
    {
        _currentTextField.text = string.Empty;
        ToggleCurrentTextFieldVisilibity(false);
    }

    private void ToggleCurrentTextFieldVisilibity(bool newState)
    {
        _currentTextField.gameObject.SetActive(newState);
    }

}
