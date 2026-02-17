using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private int _maxHealth;

    //runtime - state
    private int _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void DecreaseHealth(int HitPoints)
    {
        _currentHealth -= HitPoints;

        if (_currentHealth <= 0 )
        {
            _currentHealth = 0;
            ProcessDeath();
        }
    }

    private void ProcessDeath()
    {
        //Animation will happen here

        SceneLoader.ReloadScene();
    }
}
