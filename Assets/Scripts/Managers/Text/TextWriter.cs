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

    public void StartTypingSequence(TextSequenceId sequenceId, TextFieldsId textFieldId, bool shouldClearAtEnd, Action onFinish = null)
    {
        TextMeshProUGUI targetField = _idTextFieldMap[textFieldId];
        targetField.gameObject.SetActive(true);

        foreach (TextSequence textSequence in _levelText.TextSequences)
        {
            if (textSequence.TextSequenceId != sequenceId)
                continue;

            StartCoroutine(TypingRoutine(textSequence, targetField, shouldClearAtEnd, onFinish));
            return;
        }
    }

    private IEnumerator TypingRoutine(TextSequence textSequence, TextMeshProUGUI textField, bool shouldClearAtEnd, Action onFinish)
    {
        int textSegmentsCount = textSequence.TextSegments.Length;

        for (int segmentIndex = 0; segmentIndex < textSegmentsCount; segmentIndex++)
        {
            textField.text = string.Empty;

            TextSegment segment = textSequence.TextSegments[segmentIndex];

            foreach (char letter in segment.Text)
            {
                yield return new WaitForSeconds(segment.TimeBetweenLetters);
                textField.text += letter;
            }

            if (segmentIndex < textSegmentsCount - 1)
            {
                yield return new WaitForSeconds(segment.TimeToNextSegment);
            }
        }

        yield return new WaitForSeconds(textSequence.TimeToHideLastSegment);

        if (shouldClearAtEnd)
        {
            textField.text = string.Empty;
            textField.gameObject.SetActive(false);
        }

        onFinish?.Invoke();
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
