using System.Collections;
using TMPro;
using UnityEngine;

public class TextWriter : MonoBehaviour
{
    public static TextWriter Instance { get; private set; }

    [Header("References")]
    [SerializeField] private TextMeshProUGUI _textField;
    [SerializeField] private LevelText _levelText;

    //Run-time state
    private Coroutine _currentTypingRoutine;

    private void Awake()
    {
        Instance = this;
    }

    public void StartTypingSequence(TextSequenceId sequenceId)
    {
        foreach (TextSequence textSequence in _levelText.TextSequences)
        {
            if (textSequence.TextSequenceId == sequenceId)
            {
                if (_currentTypingRoutine != null) return;
                _currentTypingRoutine = StartCoroutine(TypingRoutine(textSequence));
                return;
            }
        }
    }

    private IEnumerator TypingRoutine(TextSequence textSequence)
    {
        int textSegmentsCount = textSequence.TextSegments.Length;
        int currentSegmentIndex = 0;
        while (currentSegmentIndex < textSegmentsCount)
        {
            _textField.text = string.Empty;

            TextSegment currentSegment = textSequence.TextSegments[currentSegmentIndex];
            float timeToNextSegment = currentSegment.TimeToNextSegment;
            float timeBetweenLetters = currentSegment.TimeBetweenLetters;

            int currentLetterIndex = 0;

            while (currentLetterIndex < currentSegment.Text.Length)
            {
                yield return new WaitForSeconds(timeBetweenLetters);

                char letterToPrint = currentSegment.Text[currentLetterIndex];
                _textField.text += letterToPrint;

                currentLetterIndex++;
            }
            currentSegmentIndex++;
            yield return new WaitForSeconds(timeToNextSegment);
        }

        _textField.text = string.Empty;
        _currentTypingRoutine = null;
    }
}
