using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RespawnSystem : MonoBehaviour
{
    public static RespawnSystem Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Transform _player;
    [SerializeField] private Level_PersistentData _persistentData;

    private List<RespawnPoint> _respawnPoints = new();
    private Dictionary<int, bool> _spawnPointUsedState = new Dictionary<int, bool>();

    private void Awake()
    {
        Instance = this;

        bool isSceneReload = SceneTracker.PreviousSceneIndex == gameObject.scene.buildIndex;

        if (!isSceneReload)
        {
            ResetPersistentData();
        }

        _player.position = _persistentData.SpawnPointPosition;
        _spawnPointUsedState = new Dictionary<int, bool>(_persistentData.SpawnPointUsedState);
    }

    private void ResetPersistentData()
    {
        _persistentData.SpawnPointPosition = Vector3.zero;
        _persistentData.IsAbilityLearnt = false;
        _persistentData.IsArtefactTaken = false;
        _persistentData.SpawnPointUsedState.Clear();
    }

    public int RegisterRespawnPoint(RespawnPoint point, int id)
    {
        _respawnPoints.Add(point);

        if (!_spawnPointUsedState.ContainsKey(id))
            _spawnPointUsedState[id] = false;

        return id;
    }

    public bool GetRespawnPointUsed(int id)
    {
        return _spawnPointUsedState.TryGetValue(id, out var used) && used;
    }

    public void SetRespawnPointUsed(int id, bool used)
    {
        _spawnPointUsedState[id] = used;
        _persistentData.SpawnPointUsedState[id] = used; // persist
    }

    public void ResetRespawnPointsForNewScene()
    {
        _respawnPoints.Clear();
        _spawnPointUsedState.Clear();
    }

    public void UpdateCurrentRespawnPoint(Vector3 newRespawnPosition)
    {
        _persistentData.SpawnPointPosition = newRespawnPosition;
    }

    public bool GetIsAbilityLearnt() => _persistentData.IsAbilityLearnt;
    public void SetIsAbilityLearnt(bool value) => _persistentData.IsAbilityLearnt = value;

    public bool GetIsArtifactTaken() => _persistentData.IsArtefactTaken;
    public void SetIsArtifactTaken(bool value) => _persistentData.IsArtefactTaken = value;
}

