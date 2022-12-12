using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
     [SerializeField]
     private float scale;

     public GameObject levelSelectSquarePrefab;

    public static float Scale { get; private set; }

    public float scrollFactor;

    public GameObject marker;

    public GameObject handle;

    public GameObject topLimit;

    public GameObject bottomLimit;

    public Text touchDeltaInfo;

    public float markerOffsetX, markerOffsetY;

    private float totalTouchDelta;
    private bool dirtyFlag;
    private const int cellsPerSquare = 4;

    private void Awake()
    {
        Scale = scale;
        totalTouchDelta = 0f;
        SpawnLevelSelectButtons();
    }

    public int GetNLevels()
    {
        Levels levels = (Levels)Resources.Load("LevelFiles/Levels");
        return levels.GetNPuzzleLevels();
    }


    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {

        HandleTouchScrolling();

    }

    public void HandleTouchScrolling()
    {
        touchDeltaInfo.text = "Total Touch delta: " + totalTouchDelta;

        if (!TouchControls.Touching && dirtyFlag)
        {

            totalTouchDelta += TouchControls.TouchDelta * .1f * scrollFactor;
          
            if (totalTouchDelta > topLimit.transform.localPosition.y)
            {
                totalTouchDelta = topLimit.transform.localPosition.y;
            }


            if (totalTouchDelta < bottomLimit.transform.localPosition.y)
            {
                totalTouchDelta = bottomLimit.transform.localPosition.y;
            }


            dirtyFlag = false;
        }

        if (TouchControls.Touching)
        {
            dirtyFlag = true;
            Vector3 newHandlePosition = handle.transform.localPosition;



            float potentialScrollY = totalTouchDelta + (TouchControls.TouchDelta * .1f * scrollFactor);

            if (potentialScrollY > topLimit.transform.localPosition.y)
            {
                potentialScrollY = topLimit.transform.localPosition.y;
            }


            if (potentialScrollY < bottomLimit.transform.localPosition.y)
            {
                potentialScrollY = bottomLimit.transform.localPosition.y;
            }
          

            newHandlePosition.y = potentialScrollY;

            handle.transform.localPosition = newHandlePosition;

        }
    }

    public void SelectLevel()
    {

    }


    public void SpawnLevelSelectButtons()
    {
        int buttonsToSpawn = GetNLevels()/cellsPerSquare;
        int i = 0;
        for (; i < buttonsToSpawn; i++)
        {
            SpawnLevelButtons(i * cellsPerSquare);
        }

        int mod = GetNLevels() % cellsPerSquare;

        if (mod > 0f) {
            SpawnLevelButtons(i*cellsPerSquare + 1, mod);
         }
        
    }


    public void SpawnLevelButtons(int startingIndex, int nCells = 4)
    {
        GameObject g = Instantiate(levelSelectSquarePrefab, marker.transform.position, Quaternion.identity);
        g.transform.SetParent(handle.transform);
        g.GetComponent<LevelSelectSquare>().SetUpCells(startingIndex, nCells);
        marker.transform.position += new Vector3(markerOffsetX, markerOffsetY, 0);
        markerOffsetX = -1 * markerOffsetX;
        topLimit.transform.position += new Vector3(0, -markerOffsetY, 0);
    }
}
