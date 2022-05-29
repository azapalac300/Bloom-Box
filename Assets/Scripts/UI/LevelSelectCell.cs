using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectCell : MonoBehaviour
{
    public CellColor unlockedColor;
    private CellColor color;

    public Image image;
    private LevelSelectSquare parentSquare;
    Position position;
    private int levelNum;
    public Text levelNumText;



    // Start is called before the first frame update
    void Start()
    {
        //PickRandomColor();
        //image.color = Cell.SelectColor(color);
        levelNumText.text = (levelNum + 1).ToString();
    }


    public void PickRandomColor()
    {

        CellColor[] colorChoices = new CellColor[] {CellColor.blue, CellColor.cyan, CellColor.red, CellColor.green,
        CellColor.orange, CellColor.purple};

        color = colorChoices[Random.Range(0, colorChoices.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadSelectedLevel()
    {
        var playSettings = Resources.Load<PlaySettings>("Settings");
        playSettings.SetCurrentLevel(levelNum);
        SceneManager.LoadScene("MainScene");
    }
    

    public void SetUp(LevelSelectSquare square, Position startPosition, int levelNum)
    {
        position = startPosition;
        parentSquare = square;
       
        transform.Translate((Vector2)transform.position - square.Center + (PositionVector * 28f));
        this.levelNum = levelNum;
        PickRandomColor();
        UpdateColor(color);
    }


    public void UpdateColor(CellColor cellColor)
    {

        UpdateSprite();
        color = cellColor;

        image.color = Cell.SelectColor(cellColor);
    }
    public void UpdateSprite()
    {
        string spriteName = "Petal";

        if (color == CellColor.wild)
        {
            spriteName += "_Rainbow-";
        }
        else if (color == CellColor.dead)
        {
            spriteName += "_Dead-";
        }
        else
        {
            spriteName += "_White-";
        }

        switch (position)
        {
            case Position.A:
                spriteName += "Top-Left";
                break;
            case Position.B:
                spriteName += "Top-Right";
                break;
            case Position.C:
                spriteName += "Bottom-Right";
                break;
            case Position.D:
                spriteName += "Bottom-Left";
                break;
        }

        Debug.Log(spriteName);
        Sprite sprite = Resources.Load<Sprite>("Petals/" + spriteName);
        image.sprite = sprite;
    }


    Vector2 positionA { get { return new Vector2(-LevelSelect.Scale / 2, LevelSelect.Scale / 2); } }
    Vector2 positionB { get { return new Vector2(LevelSelect.Scale / 2, LevelSelect.Scale / 2); } }
    Vector2 positionC { get { return new Vector2(LevelSelect.Scale / 2, -LevelSelect.Scale / 2); } }
    Vector2 positionD { get { return new Vector2(-LevelSelect.Scale / 2, -LevelSelect.Scale / 2); } }


    Vector2 PositionVector
    {
        get
        {
            switch (position)
            {
                case Position.A:
                    return positionA;

                case Position.B:
                    return positionB;

                case Position.C:
                    return positionC;

                case Position.D:
                    return positionD;

                default:
                    return new Vector2();

            }
        }
    }

}
