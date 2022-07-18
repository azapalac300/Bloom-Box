using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
public class LevelSerializer : MonoBehaviour
{
    private Game game;

    [HideInInspector]
    public Level_Data levelData;

    public bool saveLevel;
    public bool loadLevel;
    public bool newLevel;
 
    public Levels levels;

    void Awake()
    {
        game = GetComponent<Game>();
        //levelData = new Level_Data();
    }

    public void SetUpLevel()
    {
        if (levels.GetNLevels() > 0)
        {
            LoadLevel();
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (Game.mode == Game.Mode.Edit) {

            if (saveLevel)
            {
              
                SaveLevel();
                saveLevel = false;
            }

            if (loadLevel)
            {
                LoadLevel();
                loadLevel = false;

            }

            if (newLevel)
            {
                NewLevel();
                newLevel = false;
            }
        }
    }

    public void SaveLevel()
    {
        levelData.boardData = Board_Data.toData(game.board);
        StoreHand();
        levelData.tutorialData = game.tutorialText.text;
        levels.SaveLevel(game.levelNum, levelData);
    }


    public void NewLevel()
    {
        game.HideMenu();
        game.levelNum = levels.GetNLevels();
        game.maxLevels++;
        levels.AddLevel(new Level_Data());


        LoadLevel();
    }

    void LoadLevel()
    { 

        Level_Data data = levels.LoadLevel(game.levelNum);


        game.board.UpdateBoardData(data.boardData);
        game.goalsLeft = game.board.goalMarkers.Count;
        game.maxLevels = levels.GetNLevels();
        game.tutorialText.text = data.tutorialData;

        if (data.handData != null)
        {
            game.hand.UpdateHandData(data.handData);
        }
        else
        {
            game.hand.EmptyHand();
        }
        levelData = data;
    }


    public void LoadHand()
    {
        Hand_Data data = levelData.handData;
        game.hand.UpdateHandData(data);
    }
    

    public void StoreHand()
    {
        if(levelData.handData == null)
        {
            //Add the hand automatically if the list is empty
            AddHand();
            return;
        }
        Hand_Data h = Hand_Data.toData(game.hand);
        levelData.handData = h;
    }


    public void AddHand()
    {
       
        Hand_Data h = Hand_Data.toData(game.hand);
          


            if (game.hand == null)
            {
                throw new System.Exception("Hand does not exist");
            }

        levelData.handData = h;
    }


}

