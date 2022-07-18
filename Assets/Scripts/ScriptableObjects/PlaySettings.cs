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
    private int highestUnlockedLevel;

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

    public int GetHighestUnlockedLevel()
    {

        if (PlayerPrefs.HasKey("HighestUnlockedLevel"))
        {

            highestUnlockedLevel = PlayerPrefs.GetInt("HighestUnlockedLevel");

            return highestUnlockedLevel;
        }
        else
        {
            PlayerPrefs.SetInt("HighestUnlockedLevel", GetCurrentLevel());
            highestUnlockedLevel = GetCurrentLevel();
            return highestUnlockedLevel;
        }
    }


    public void SetCurrentLevel(int levelNum, bool editMode = false)
    {
        PlayerPrefs.SetInt("CurrentLevel", levelNum);

        currentLevel = levelNum;

        if(levelNum >= GetHighestUnlockedLevel() && !editMode)
        {
            PlayerPrefs.SetInt("HighestUnlockedLevel", levelNum);
            highestUnlockedLevel = levelNum;
        }
    }

    public void FullReset()
    {
        PlayerPrefs.SetInt("HighestUnlockedLevel", 0);
        highestUnlockedLevel = 0;

        PlayerPrefs.SetInt("CurrentLevel", 0);
        currentLevel = 0;
    }



}
