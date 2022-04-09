using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSquare : MonoBehaviour
{
    public GameObject levelSelectCellPrefab;
    public LevelSelectCell cell1, cell2, cell3, cell4;
    // Start is called before the first frame update
    void Start()
    {
        SpawnCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnCells()
    {
        GameObject cell = Instantiate(levelSelectCellPrefab, transform.position, Quaternion.identity);
        cell.transform.SetParent(transform);
    }
}
