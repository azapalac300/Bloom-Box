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

    public void SetUpPuzzleLevel()
    {
        if (levels.GetNPuzzleLevels() > 0)
        {
            LoadLevel(Game.Mode.Puzzle);
        }
    }


    public void SetUpEndlessLevel()
    {
        if(levels.GetNEndlessLevels() > 0)
        {
            LoadLevel(Game.Mode.Endless);
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
                LoadLevel(Game.Mode.Puzzle);
                loadLevel = false;

            }

            if (newLevel)
            {
                NewPuzzleLevel();
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


    public void NewPuzzleLevel()
    {
        game.HideMenu();
        game.levelNum = levels.GetNPuzzleLevels();
        game.maxLevels++;
        levels.AddLevel(new Level_Data(), Game.Mode.Puzzle);


        LoadLevel(Game.Mode.Puzzle);
    }


    public void NewEndlessLevel()
    {
        game.HideMenu();
        game.endlessLevelNum = levels.GetNEndlessLevels();
        game.maxEndlessLevels++;
        levels.AddLevel(new Level_Data(), Game.Mode.Endless);


        LoadLevel(Game.Mode.Endless);
    }


    void LoadLevel(Game.Mode mode)
    {
        int levelNum = -1;
        if (mode == Game.Mode.Puzzle)
        {
            levelNum = game.levelNum;
        }
        else if (mode == Game.Mode.Endless)
        {
            levelNum = game.endlessLevelNum;
        }
        else
        {
            Debug.LogError(mode + " is not a valid play mode");
            return;
        }


        Level_Data data = levels.LoadLevel(levelNum, mode);


        game.board.UpdateBoardData(data.boardData);
        game.goalsLeft = game.board.goalMarkers.Count;

        if (mode == Game.Mode.Puzzle)
        {
            game.maxLevels = levels.GetNPuzzleLevels();
        }else if(mode == Game.Mode.Endless){
            game.maxLevels = levels.GetNEndlessLevels();
        }
        else
        {
            Debug.LogError(mode + " is not a valid play mode");
            return;
        }

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

