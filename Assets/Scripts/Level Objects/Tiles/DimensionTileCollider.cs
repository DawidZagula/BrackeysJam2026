using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DimensionTileCollider : MonoBehaviour
{
    //References
    TilemapCollider2D _tilemapCollider;
    TilemapRenderer _tilemapRenderer;

    private void Awake()
    {
        _tilemapCollider = GetComponent<TilemapCollider2D>();
        _tilemapRenderer = GetComponent<TilemapRenderer>();
    }

    private void Update()
    {
        ToggleCollider();
    }

    private void ToggleCollider()
    {
        _tilemapCollider.enabled = _tilemapRenderer.enabled ? true : false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerHealth playerHealth))
        {
            playerHealth.DecreaseHealth(1);
        }
    }
}
