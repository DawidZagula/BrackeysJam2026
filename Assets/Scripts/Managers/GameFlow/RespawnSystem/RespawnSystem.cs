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
        _persistentData.SpawnPointPosition = new Vector3(0, 0, 0);
        _persistentData.IsAbilityLearnt = false;
        _persistentData.IsArtefactTaken = false;
    }

    public void UpdateCurrentRespawnPoint(Vector3 newRespawnPosition)
    {
        _persistentData.SpawnPointPosition = newRespawnPosition;
    }
    public bool GetIsAbilityLearnt()
    {
        return _persistentData.IsAbilityLearnt;
    }

    public void SetIsAbilityLearnt(bool isAbilityLearnt)
    {
        _persistentData.IsAbilityLearnt = isAbilityLearnt;
    }

    public bool GetIsArtifactTaken()
    {
        return _persistentData.IsArtefactTaken;
    }

    public void SetIsArtifactTaken(bool isArtifactTaken)
    {
        _persistentData.IsArtefactTaken = isArtifactTaken;
    }
}

