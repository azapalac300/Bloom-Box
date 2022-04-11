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

    public void PlayGame()
    {
        StartSceneTransition?.Invoke(sceneLoadTime);
        settings.loadedFromMenu = true;
        StartCoroutine(LoadMainScene());
    }

    IEnumerator LoadMainScene()
    {
        yield return new WaitForSeconds(sceneLoadTime);
        SceneManager.LoadScene("MainScene");
    }

    public void LoadCredits()
    {
        StartSceneTransition.Invoke(sceneLoadTime);
        StartCoroutine(LoadCreditsScene());
    }

    IEnumerator LoadCreditsScene()
    {
        yield return new WaitForSeconds(sceneLoadTime);
        SceneManager.LoadScene("Credits");
    }

    public void LoadMenu()
    {
        StartSceneTransition?.Invoke(sceneLoadTime);
        StartCoroutine(LoadMenuScene());
    }

    IEnumerator LoadMenuScene()
    {
        yield return new WaitForSeconds(sceneLoadTime);
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevels()
    {
        StartSceneTransition.Invoke(sceneLoadTime);
        StartCoroutine(LoadLevelsScene());
    }

    IEnumerator LoadLevelsScene()
    {
        yield return new WaitForSeconds(sceneLoadTime);
        SceneManager.LoadScene("LevelSelect");
    }
}
