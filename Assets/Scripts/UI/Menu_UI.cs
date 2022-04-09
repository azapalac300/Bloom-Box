using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu_UI : MonoBehaviour
{
    public PlaySettings settings;
    public void PlayGame()
    {
        settings.loadedFromMenu = true;
        SceneManager.LoadScene("MainScene");
    }


    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void LoadLevels()
    {
        SceneManager.LoadScene("LevelSelect");
    }
}
