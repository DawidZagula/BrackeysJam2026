using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using Zenject;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int _maxHealth;

    //References
    private GameStateManager _gameStateManager;
    private PlayerVisual _playerVisual;
    private PlayerMover _playerMover;

    //runtime - state
    private int _currentHealth;
    private bool _isDead;

    [Inject]
    public void Construct(GameStateManager gameStateManager)
    {
        _gameStateManager = gameStateManager;
    }

    private void Awake()
    {
        _currentHealth = _maxHealth;
        _playerVisual = GetComponentInChildren<PlayerVisual>();
        _playerMover = GetComponent<PlayerMover>();
    }

    public void DecreaseHealth(int HitPoints)
    {
        _currentHealth -= HitPoints;

        if (_currentHealth <= 0 )
        {
            _currentHealth = 0;
            if (_isDead) { return; }
            ProcessDeath();
        }
    }

    public void ProcessDeath()
    {
        _isDead = true;

        const float cameraShakeMultiplier = 3f;
        CameraShaker.Instance.ShakeCamera(cameraShakeMultiplier);

        _gameStateManager.ChangeCurrentState(GameState.GameOver);
        _playerMover.StopAllMovement();
        
        AudioPlayer.Instance.PlaySound(AudioPlayer.AudioName.Death);

        _playerVisual.PlayAnimation("PlayerDeath", CallReloadScene);
    }

    private void CallReloadScene()
    {
        SceneLoader.ReloadScene();
    }
}
