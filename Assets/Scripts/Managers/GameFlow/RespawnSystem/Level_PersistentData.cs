using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level_PersistentData")]
public class Level_PersistentData : ScriptableObject
{
    [field: SerializeField] public Vector3 SpawnPointPosition { get; set; }
    [field: SerializeField] public bool IsAbilityLearnt { get; set; }
}
