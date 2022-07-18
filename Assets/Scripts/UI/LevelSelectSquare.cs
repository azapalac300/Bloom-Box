using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSquare : MonoBehaviour
{
    public GameObject levelSelectCellPrefab;
    public LevelSelectCell cell1, cell2, cell3, cell4;
    
    public LevelSelectCell[] cells { get { 
            return new LevelSelectCell[] { cell1, cell2, cell3, cell4 };  
        } }

    public float scale;

    public Vector2 Center { get { return transform.position; } }

    public void SetUpCells(int startingIndex, int nCells = 4)
    {
        for (int i = 0; i < nCells; i++)
        {
            cells[i] = InitializeCell((Position)i, startingIndex + i);
        }
        
    }


    LevelSelectCell InitializeCell(Position position, int index)
    {
        LevelSelectCell cell = Instantiate(levelSelectCellPrefab, transform.position, Quaternion.identity).GetComponent<LevelSelectCell>();
        cell.transform.localScale *= LevelSelect.Scale / 4;
        cell.transform.SetParent(transform);
        cell.SetUp(this, position, index);
        return cell;
    }


}
