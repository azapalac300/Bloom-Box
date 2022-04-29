using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
     [SerializeField]
     private float scale;

     public GameObject levelSelectSquarePrefab;

     public static float Scale { get; private set; }

    private int nLevels;

    public GameObject marker;

    private GameObject mask;

    public float markerOffsetX, markerOffsetY;

    private void Awake()
    {
        Scale = scale;
        mask = GameObject.Find("Mask");


        SpawnLevelSelectButtons();
    }

    public int GetNLevels()
    {
        return 2;
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

    }
}
