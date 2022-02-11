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

}
