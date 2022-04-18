using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Game : MonoBehaviour {

    [SerializeField]
    private float scale;

    [SerializeField]
    private float rotateSpeed;

    [SerializeField]
    private float shiftSpeed;

    [SerializeField]
    private int boardSize;

    [SerializeField]
    private Mode gameMode;

    public int levelNum;


    public int maxLevels;

    public int goalsLeft;

    public GameObject playButtons;
    public GameObject playMenu;
    public GameObject editMenu;

    public GameObject victoryDisplay;

    public Text tutorialText;

    public GameObject editIndicator;
    public GameObject testIndicator;

    public PlaySettings settings;

    public InputField input;

    public static float Scale { get; private set; }
    public static float RotateSpeed { get; private set; }

    public static float ShiftSpeed { get; private set; }

    public static int BoardSize { get; private set; }
    
    public static Square SelectedSquare { get; private set; }

    public static Vector2 worldTouchPosition { get {

           return Camera.main.ScreenToWorldPoint(TouchControls.mainTouchPos);

    } }



    public static string CoordsToString(int[] coords)
    {
        return coords[0].ToString() + " " + coords[1].ToString();
    }

    public void IncrementLevel()
    {
        if(levelNum < maxLevels -  1)
        {
            levelNum++;
        }
    }

    public static bool touching { get { return Input.GetMouseButton(0); } }


    Vector3 origSelectedSquarePosition;
    public Hand hand;
    public Board board;
    private LevelSerializer levelSerializer;
    
    public enum Mode
    {
        Play,
        Test,
        Edit
    }
    public static Mode mode { get; private set; }

   
    private void Awake()
    {
        if (settings.loadedFromMenu)
        {
            levelNum = settings.GetCurrentLevel();
            settings.testMode = false;
        }
        else
        {
            settings.testMode = true;
        }

     
        Scale = scale;
        RotateSpeed = rotateSpeed;
        ShiftSpeed = shiftSpeed;
        BoardSize = boardSize;
        mode = gameMode;
        levelSerializer = GetComponent<LevelSerializer>();

      
    }

    public void ShowMenu()
    {
        
        if (mode == Mode.Play)
        {
            playMenu.SetActive(true);

        }
        else if (mode == Mode.Edit)
        {
            editMenu.SetActive(true);
        }

        board.gameObject.SetActive(false);
        hand.gameObject.SetActive(false);
        playButtons.SetActive(false);
    }


    public void HideMenu()
    {
        if (mode == Mode.Play)
        {
            playMenu.SetActive(false);

        }
        else if (mode == Mode.Edit)
        {
            editMenu.SetActive(false);
        }

        board.gameObject.SetActive(true);
        hand.gameObject.SetActive(true);
        playButtons.SetActive(true);
    }

    // Use this for initialization
    void Start () {

        
        board.SetUp();
        levelSerializer.SetUpLevel();
        SwitchMode();
        

    }

    public void SwitchMode() {

        gameMode = mode;

        switch (mode)
        {
            case Mode.Edit:
                ResetLevel();
                playButtons.SetActive(true);
                testIndicator.SetActive(false);
                editIndicator.SetActive(true);
                board.SetUpEdit();
                break;
            case Mode.Play:
                playButtons.SetActive(true);
                testIndicator.SetActive(false);
                editIndicator.SetActive(false);
                board.SetUpPlay();
                break;
            case Mode.Test:
                playButtons.SetActive(false);
                testIndicator.SetActive(true);
                editIndicator.SetActive(false);
                board.SetUpPlay();
                break;
        }
       
    }

	// Update is called once per frame
	void Update () {



        if ((mode == Mode.Play || mode == Mode.Test))
        {
            PlayGame();


            
        }else if (mode == Mode.Edit)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                levelSerializer.SaveLevel();
                mode = Mode.Test;
                SwitchMode();
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                levelSerializer.SaveLevel();
            }

            board.EditLevel();
        }
        
	}



    public void PlayGame()
    {
        if (mode == Mode.Test)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                mode = Mode.Edit;
                SwitchMode();
            }
        }

        if (Game.SelectedSquare != null)
        {
            board.PlaceOnBoard(Game.SelectedSquare);
            if (!touching)
            {
                SelectedSquare.Deselect(true);
                SelectedSquare = null;
            }
        }
    }


    public static Vector2 MatchOffset(MatchDirection d)
    {
        Vector2 v = Vector2.zero;
        switch (d)
        {
            //Need to offset in the opposite direction of what I match with
            case MatchDirection.right:
                v = Vector2.left;
                break;
            case MatchDirection.up:
                v = Vector2.down;
                break;
            case MatchDirection.left:
                v = Vector2.right;
                break;
            case MatchDirection.down:
                v = Vector2.up;
                break;


        }
        return v * Game.Scale *2; 
    }


    public static void SelectSquare(Square s)
    {
        if ((mode == Mode.Play || mode == Mode.Test) && touching)
        {
            if (SelectedSquare != null)
            {
                SelectedSquare.Deselect(true);
                
            }

            SelectedSquare = s;
            s.squareAudio.PlayPickupSound();
            s.selected = true;
        }
    }

    public void WinLevel()
    {

         victoryDisplay.SetActive(true);

         Invoke("LoadNextLevel", 2f);
        
    }

    public void ResetLevel()
    {
        HideMenu();
        levelSerializer.SetUpLevel();
    }

    public void LoadNextLevel()
    {
        if(mode == Mode.Edit)
        {
            Debug.LogError("Something went wrong, you shouldn't be able to access this function in edit mode");
        }

        if (mode == Mode.Play)
        {

            victoryDisplay.SetActive(false);
            if (levelNum < maxLevels - 1)
            {

                levelNum++;

                if (settings.GetCurrentLevel() < maxLevels - 1 && !settings.testMode)
                {
                    settings.SetCurrentLevel(settings.GetCurrentLevel() + 1);
                }


                levelSerializer.SetUpLevel();
            }
            else
            {
                //Return to the menu if we've beaten the game
                SceneManager.LoadScene("MainMenu");
            }
        }


        if(mode == Mode.Test)
        {
            levelSerializer.SetUpLevel();
        }
    }

    public static Vector3 Scale2D()
    {
        return new Vector3(Game.Scale, Game.Scale, 1);
    }

    public void LoadLevelFromUI()
    {
        HideMenu();
        string s = input.text;
        LoadLevel(int.Parse(s));
    }

    private void LoadLevel(int levelToLoad)
    {
        if (levelToLoad < maxLevels)
        {
            levelNum = levelToLoad;
            settings.SetCurrentLevel(levelToLoad);
            levelSerializer.SetUpLevel();
        }
        else
        {
            Debug.LogError("Level " + levelToLoad + " does not exist!");
        }
    }
}

