using UnityEngine;

public class DestructorOnArtefactTaken : MonoBehaviour
{
    private void Start()
    {
        if (RespawnSystem.Instance.GetIsArtifactTaken())
        {
            Destroy(gameObject);
        }
    }
}
