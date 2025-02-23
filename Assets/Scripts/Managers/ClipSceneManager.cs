using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClipSceneManager : MonoBehaviour
{
    public void StartAdvertisement()
    {
        StartCoroutine(LoadClipScene(1));
    }
    
    public void ShowEndScene()
    {
        StartCoroutine(LoadClipScene(2));
    }
    
    public void ExitExperience()
    {
        Application.Quit();
    }

    private IEnumerator LoadClipScene(int sceneNumber)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation loadingOperation = SceneManager.LoadSceneAsync(sceneNumber);
        loadingOperation.allowSceneActivation = false;
        while (!loadingOperation.isDone)
        {
            if (loadingOperation.progress >= 0.9f) loadingOperation.allowSceneActivation = true;
            yield return null;
        }
        yield return SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(sceneNumber));
        yield return SceneManager.UnloadSceneAsync(currentScene.buildIndex);
    }
}
