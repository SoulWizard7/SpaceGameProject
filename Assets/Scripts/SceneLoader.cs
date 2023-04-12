using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum EnumScene
{
    MainMenu,
    InsideShip,
    Loading,
    SolarSystem,
    EnemyShipTest,
    EnemyShipTestLevelSetupManager,
    Ship
}

public static class SceneLoader
{
    private class LoadingMonoBehaviour : MonoBehaviour {}

    private static Action onLoaderCallback;
    private static AsyncOperation loadingAsyncOperation;

    public static void Load(EnumScene enumScene)
    {
        onLoaderCallback = () =>
        {
            GameObject loadingGameObject = new GameObject("Loading Game Object");
            loadingGameObject.AddComponent<LoadingMonoBehaviour>().StartCoroutine(LoadScene(enumScene));
        };
        
        SceneManager.LoadScene(EnumScene.Loading.ToString());
    }

    private static IEnumerator LoadScene(EnumScene enumScene)
    {
        yield return null;
        loadingAsyncOperation = SceneManager.LoadSceneAsync(enumScene.ToString());

        while (!loadingAsyncOperation.isDone)
        {
            yield return null;
        }
    }

    public static float GetLoadingProgress()
    {
        if (loadingAsyncOperation != null)
        {
            return loadingAsyncOperation.progress;
        }
        return 1f;
    }

    public static void LoaderCallback()
    {
        if (onLoaderCallback != null)
        {
            onLoaderCallback();
            onLoaderCallback = null;
        }
    }
}
