using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlaySettings : ScriptableObject
{
    
    public bool loadedFromMenu;
    public bool testMode;

    [SerializeField]
    private int currentLevel;

    [SerializeField]
    private float volume;

    public int GetCurrentLevel()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
           
            currentLevel = PlayerPrefs.GetInt("CurrentLevel");

            return currentLevel;
        }
        else
        {
            PlayerPrefs.SetInt("CurrentLevel", 0);
            currentLevel = 0;
            return 0;
        }

    }


    public void SetCurrentLevel(int levelNum)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelNum);

        currentLevel = levelNum;
    }


}
