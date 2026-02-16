using UnityEngine;

public class SceneTransitionTrigger : MonoBehaviour
{
    [Header("Configuration")]
    [SerializeField] private GameScene _nextScene;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerMover player))
        {
            SceneLoader.ProcessLoadScene(_nextScene);
        }
    }
}
