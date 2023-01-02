using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Menu_UI : MonoBehaviour
{
    public PlaySettings settings;

    public float sceneLoadTime;

    public static Action<float> StartSceneTransition;

    public PrevScene prevScene;

    public void Awake()
    {
        prevScene = Resources.Load<PrevScene>("PrevScene");
    }

    public void PlayGame()
    {
        settings.loadedFromMenu = true;
        LoadSceneDelayed("MainScene");
    }

    public void LoadCredits()
    {
        LoadSceneDelayed("Credits");
    }

    public void LoadMenu()
    {
        LoadSceneDelayed("MainMenu");
    }

    public void LoadLevels()
    {
        LoadSceneDelayed("LevelSelect");
    }

    public void LoadPrevScene()
    {
        LoadSceneDelayed(prevScene.prevSceneName);
    }

    public void LoadSceneDelayed(string sceneName)
    {
        prevScene.prevSceneName = SceneManager.GetActiveScene().name;
        StartSceneTransition.Invoke(sceneLoadTime);
        StartCoroutine(LoadScene(sceneName));
    }

    IEnumerator LoadScene(String sceneName)
    {
        yield return new WaitForSeconds(sceneLoadTime);
        SceneManager.LoadScene(sceneName);
    }

    public void OpenKofiSite()
    {
        Application.OpenURL("https://ko-fi.com/alexz68053");
    }
}
