using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnSystem : MonoBehaviour
{
    public static RespawnSystem Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform _player;

    [SerializeField] private Level_PersistentData _persistentData;


    private void Awake()
    {
        Instance = this;

        bool isSceneReload =
            SceneTracker.PreviousSceneIndex == gameObject.scene.buildIndex;

        if (!isSceneReload)
        {
            ResetPersistentData();
        }

        _player.position = _persistentData.SpawnPointPosition;
    }

    private void ResetPersistentData()
    {
        _persistentData.SpawnPointPosition = Vector3.zero;
        _persistentData.IsAbilityLearnt = false;
    }

    public void UpdateCurrentRespawnPoint(Vector3 newRespawnPosition)
    {
        _persistentData.SpawnPointPosition = newRespawnPosition;
    }

    public void SetIsAbilityLearnt(bool isAbilityLearnt)
    {
        _persistentData.IsAbilityLearnt = isAbilityLearnt;
    }

    public bool GetIsAbilityLearnt()
    {
        return _persistentData.IsAbilityLearnt;
    }
}

