using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
     [SerializeField]
     private float scale;

     public GameObject levelSelectSquarePrefab;

     public static float Scale { get; private set; }


    public GameObject marker;

    public GameObject mask;

    public float markerOffsetX, markerOffsetY;

    private void Awake()
    {
        Scale = scale;

        SpawnLevelSelectButtons();
    }

    public int GetNLevels()
    {
        return 16;
    }


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLevel()
    {

    }


    public void SpawnLevelSelectButtons()
    {
        int buttonsToSpawn = GetNLevels()/4;

        for(int i = 0; i < buttonsToSpawn; i++)
        {
            GameObject g = Instantiate(levelSelectSquarePrefab, marker.transform.position, Quaternion.identity);
            g.transform.SetParent(mask.transform);
            g.GetComponent<LevelSelectSquare>().SetUpCells(i*4);
            marker.transform.position += new Vector3(markerOffsetX, markerOffsetY, 0);
            markerOffsetX = -1 * markerOffsetX;
        }
    }
}
