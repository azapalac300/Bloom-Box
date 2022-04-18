using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectSquare : MonoBehaviour
{
    public GameObject levelSelectCellPrefab;
    public LevelSelectCell cell1, cell2, cell3, cell4;

    public float scale;

    public Vector2 Center { get { return transform.position; } }

    // Start is called before the first frame update
    void Start()
    {
        SetUpCells();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetUpCells()
    {
        cell1 = InitializeCell(Position.A);
        cell2 = InitializeCell(Position.B);
        cell3 = InitializeCell(Position.C);
        cell4 = InitializeCell(Position.D);

    }


    LevelSelectCell InitializeCell(Position position)
    {
        LevelSelectCell cell = Instantiate(levelSelectCellPrefab, transform.position, Quaternion.identity).GetComponent<LevelSelectCell>();
        cell.transform.localScale *= LevelSelect.Scale / 4;
        cell.transform.parent = transform;
        cell.SetUp(this, position);
        return cell;
    }


}
