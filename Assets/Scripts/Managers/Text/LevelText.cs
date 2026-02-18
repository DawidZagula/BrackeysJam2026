using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/LevelTextSO")]
public class LevelText : ScriptableObject
{
    [field: SerializeField] public TextSequence[] TextSequences {  get; private set; }
}

[Serializable]
public class TextSequence
{
    [field:SerializeField] public string Id {  get; private set; }
    [field: SerializeField] public TextSequenceId TextSequenceId { get; private set; }
    [field: SerializeField] public TextSegment[] TextSegments { get; private set; }
    [field: SerializeField] public float TimeToHideLastSegment { get; private set; }
}

[Serializable]
public class TextSegment
{
    [field: SerializeField] public string Text { get; private set; }
    [field: SerializeField] public float TimeBetweenLetters { get; private set; } = .2f;
    [field: SerializeField] public float TimeToNextSegment { get; private set; } = 2f;
}
