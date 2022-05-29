using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSquare : MonoBehaviour
{
    public GameObject levelSelectCellPrefab;
    public LevelSelectCell cell1, cell2, cell3, cell4;

    public float scale;

    public Vector2 Center { get { return transform.position; } }

    public void SetUpCells(int startingIndex)
    {
        cell1 = InitializeCell(Position.A, startingIndex);
        cell2 = InitializeCell(Position.B, startingIndex + 1);
        cell4 = InitializeCell(Position.D, startingIndex + 2);
        cell3 = InitializeCell(Position.C, startingIndex + 3);
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
