using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hand : MonoBehaviour {
    public GameObject squarePrefab;
    public GameObject handPositionMarker;
    public Text squareCountText;

    public GameObject squaresTray;
    public GameObject handEditControls;

    public int handSize;
    public bool resetHand;
    public bool addHand;
    public bool loadHand;

    private const int MAX_SQUARES_PER_LINE = 6;

    public static Bounds trayBounds { get; private set; }
    public List<Square> squaresInHand;

    private float xOffset, yOffset;
    private int squareCount;
    private Vector2 origPosition;
    private LevelSerializer levelSerializer;
    

	// Use this for initialization
	void Awake () {
        squaresInHand = new List<Square>();
        origPosition = handPositionMarker.transform.position;
        levelSerializer = GameObject.Find("Game").GetComponent<LevelSerializer>();
        trayBounds = squaresTray.GetComponent<BoxCollider>().bounds;

        xOffset = Game.Scale * 3f;
        yOffset = Game.Scale * 2.5f;
        squareCount = 0;
	}

    public void DrawNewSquare()
    {
        Vector2 position = handPositionMarker.transform.position;

        
        if (squaresInHand.Count < handSize)
        {
            Square square = Instantiate(squarePrefab, transform.position, Quaternion.identity).GetComponent<Square>();
            squaresInHand.Add(square);
            square.transform.position = position;
            square.transform.parent = transform;
            square.SetUpCells();
            squareCount++;
            MoveMarkerForward(squareCount);
        }
        
    }

    private void MoveMarkerForward(int cellCount)
    {
        handPositionMarker.transform.Translate(new Vector2(xOffset, 0));

        if(cellCount % 6 == 0f && cellCount > 0)
        {
            handPositionMarker.transform.position = origPosition;
            handPositionMarker.transform.Translate(new Vector2(0, -yOffset));
        }
    }

    private void MoveMarkerBack()
    {
        handPositionMarker.transform.Translate(new Vector2(-xOffset, 0));
    }

    public void RemoveLastSquare()
    {
        handSize--;
        DeleteSquare(squaresInHand[squaresInHand.Count - 1]);
    }


    public void AddNextSquare()
    {
        handSize++;
        DrawNewSquare();
    }

    public void UpdateHandData(Hand_Data data)
    {
        handSize = data.handSquares.Count;
        ResetHand();

        for(int i = 0; i < handSize; i++)
        {
            squaresInHand[i].UpdateData(data.handSquares[i]);
        }

    }

    public void EmptyHand()
    {
        handSize = 0;
        ResetHand();
    }


    private void Fill()
    {
        handPositionMarker.transform.position = origPosition;
        for(int i = 0; i < handSize; i++)
        {
            DrawNewSquare();
        }
    }

    public void RemoveSquare(Square square)
    {
        squaresInHand.Remove(square);
    }

    public void DeleteSquare(Square square)
    {
        squaresInHand.Remove(square);
        Destroy(square.gameObject);
        MoveMarkerBack();
    }

    // Update is called once per frame
    void Update () {
        if( Game.mode == Game.Mode.Edit)
        {
            if (resetHand)
            {
                ResetHand();
                resetHand = false;
            }

            if (addHand)
            {
                levelSerializer.AddHand();
                addHand = false;
            }

            if (loadHand)
            {
                levelSerializer.LoadHand();
                loadHand = false;
            }
            handEditControls.SetActive(true);
        }
        else
        {
            handEditControls.SetActive(false);
        }

        squareCountText.text = "Square Count: " + squareCount;
	}

    


    void ResetHand()
    {
        
        for (int i = 0; i < squaresInHand.Count; i++)
        {
            Destroy(squaresInHand[i].gameObject);
        }

        squaresInHand.Clear();
        squareCount = 0;
        Fill();
    }
}
 