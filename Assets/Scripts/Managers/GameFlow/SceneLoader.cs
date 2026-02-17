using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void ProcessLoadScene(GameScene sceneToLoad)
    {
        switch (sceneToLoad)
        {
            case GameScene.MainMenu:
                FadeTransitionManager.Instance.FadeOut(()=> LoadScene(0));
                break;
            case GameScene.Level_1:
                FadeTransitionManager.Instance.FadeOut(() => LoadScene(1));

                break;
            case GameScene.Level_2:
                FadeTransitionManager.Instance.FadeOut(() => LoadScene(2));

                break;
            case GameScene.Level_3:
                FadeTransitionManager.Instance.FadeOut(() => LoadScene(3));

                break;
        }
    }

    private static void LoadScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
