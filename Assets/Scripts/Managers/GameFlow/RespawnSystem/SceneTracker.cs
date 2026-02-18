using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneTracker
{
    public static int PreviousSceneIndex { get; private set; }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void Initialize()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PreviousSceneIndex = scene.buildIndex;
    }
}
