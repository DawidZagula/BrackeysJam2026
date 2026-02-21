using UnityEngine;

public static class CutsceneStateTracker
{
    private static int _lastCutsceneSceneIndex = -1;

    public static bool HasPlayedInScene(int sceneIndex)
    {
        return _lastCutsceneSceneIndex == sceneIndex;
    }
    public static void MarkPlayed(int sceneIndex)
    {
        _lastCutsceneSceneIndex = sceneIndex;
    }
}

