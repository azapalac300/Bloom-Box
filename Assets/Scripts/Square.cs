using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[ExecuteInEditMode]
public class Square : MonoBehaviour {
    public GameObject cellPrefab;

    public bool selected;
    public bool debugDist;
    public bool placed;
    public bool tray;
    public bool inTray { get { return Hand.trayBounds.Intersects(GetComponent<BoxCollider>().bounds); } }
    public bool canBePlaced;

    public bool highlighted;
    private Vector3 selectedScale;
    private Vector3 origScale;

    private GameObject icon;

    public event Action<Square> PlaceSquareOnBoard;

    [SerializeField]
    private SquareType typeSelection;

    public SquareType type { get; private set; }

    [SerializeField]
    private CellColor aColor, bColor, cColor, dColor;
    
    [HideInInspector]
    public Cell cellA, cellB, cellC, cellD;

    [HideInInspector]
    public int[] coords;

    

    private bool cellsSetUp = false;
    private Vector3 origPosition;
    public bool rotating { get; private set; }
    public bool shifting;

    private Vector3 moveDelta;
    

    private MatchDirection moveDirection;

    public float radius { get { return Game.Scale * 5f; } }
    public Vector2 flatPosition { get { return ResourceManager.Flatten(transform.position); } }



    public List<Cell> Cells {
        get {
            return new List<Cell>
            {
                cellA, cellB, cellC, cellD
            };
        }
    }

    public Vector2 Center { get { return transform.position; } }
    // Use this for initialization 
    //Initialize all cells
    void Start() {

        SetUpCells();
        UpdateAbiltyIcon();
    }


    public void UpdateAbiltyIcon()
    {
        if (type == typeSelection)
        {
            return;
        }
        else
        {
            type = typeSelection;
            Destroy(icon);
        }

        GameObject iconCandidate = null;
        switch (type)
        {
            case SquareType.normal:
                return;
            #region paint abilities
            case SquareType.paint_red:
                iconCandidate = Resources.Load<GameObject>("Paint");
                iconCandidate.GetComponentInChildren<PaintBrush>().color = CellColor.red;
                break;

            case SquareType.paint_blue:
                iconCandidate = Resources.Load<GameObject>("Paint");
                iconCandidate.GetComponentInChildren<PaintBrush>().color = CellColor.blue;
                break;
            case SquareType.paint_cyan:
                iconCandidate = Resources.Load<GameObject>("Paint");
                iconCandidate.GetComponentInChildren<PaintBrush>().color = CellColor.cyan;
                break;
            case SquareType.paint_green:
                iconCandidate = Resources.Load<GameObject>("Paint");
                iconCandidate.GetComponentInChildren<PaintBrush>().color = CellColor.green;
                break;
            case SquareType.paint_orange:
                iconCandidate = Resources.Load<GameObject>("Paint");
                iconCandidate.GetComponentInChildren<PaintBrush>().color = CellColor.orange;
                break;
            case SquareType.paint_purple:
                iconCandidate = Resources.Load<GameObject>("Paint");
                iconCandidate.GetComponentInChildren<PaintBrush>().color = CellColor.purple;
                break;
            case SquareType.paint_dead:
                iconCandidate = Resources.Load<GameObject>("Paint");
                iconCandidate.GetComponentInChildren<PaintBrush>().color = CellColor.dead;
                break;
            case SquareType.paint_wild:
                iconCandidate = Resources.Load<GameObject>("RainbowPaint");
                break;
            #endregion
            #region other abilities
            case SquareType.rotate_right:
                iconCandidate = Resources.Load<GameObject>("RotateRight");
                break;

            case SquareType.rotate_left:
                iconCandidate = Resources.Load<GameObject>("RotateLeft");
                break;

            case SquareType.wind_up:
                iconCandidate = Resources.Load<GameObject>("WindUp");
                break;

            case SquareType.wind_down:
                iconCandidate = Resources.Load<GameObject>("WindDown");
                break;

            case SquareType.wind_left:
                iconCandidate = Resources.Load<GameObject>("WindLeft");
                break;

            case SquareType.wind_right:
                iconCandidate = Resources.Load<GameObject>("WindRight");
                break;
                #endregion
        }

        if (iconCandidate != null)
        {
            icon = Instantiate(iconCandidate, transform.position + new Vector3(0, 0, -1f), Quaternion.identity);
            icon.transform.localScale = Game.Scale2D() * 1.5f;
            icon.transform.SetParent(transform);
        }
    }

    public void SetUpCells()
    {
        if (!cellsSetUp)
        {
            cellA = InitializeCell(Position.A);
            cellB = InitializeCell(Position.B);
            cellC = InitializeCell(Position.C);
            cellD = InitializeCell(Position.D);


            origScale = transform.localScale;
            selectedScale = origScale * 1.2f;

            origPosition = transform.position;
            GetComponent<BoxCollider>().size = new Vector3(Game.Scale * 2, Game.Scale * 2, 1);

            if (Game.mode == Game.Mode.Edit)
            {
                SetBlank();
            }
            cellsSetUp = true;
        }
    }

    Cell InitializeCell(Position position)
    {
        Cell cell = Instantiate(cellPrefab, transform.position, Quaternion.identity).GetComponent<Cell>();
        cell.transform.localScale *= Game.Scale / 4;
        cell.transform.parent = this.transform;
        cell.SetUp(this, position);

        
        //cell.SetRandomColor();
        return cell;
    }

    // Update is called once per frame
    void Update() {


        coords = Board.GetGridCoordinates(transform.position);


        if (highlighted && !rotating)
        {
            transform.localScale = selectedScale;
        }
        else
        {
            transform.localScale = origScale;
        }
        

        //Can be shortened because all cells shift at once
        tray = inTray;
        if (Input.GetKeyDown(KeyCode.R) && !rotating && selected)
        {
            transform.localScale = origScale;
            FWDRotate();
        }


        if (Input.GetKeyDown(KeyCode.Q) && !rotating && selected)
        {
            transform.localScale = origScale;
            BWDRotate();
        }

        rotating = (cellD.LeftShifting || cellD.RightShifting);

        if (shifting)
        {
            transform.Translate(moveDelta * Game.ShiftTime * Time.deltaTime);
            Debug.DrawLine(transform.position, transform.position + moveDelta*10, Color.red);
            if (CheckShiftCollision(moveDirection))
            {
                shifting = false;
                PlaceSquareOnBoard?.Invoke(this) ;
                
            }
        }


        if (Game.mode == Game.Mode.Play || Game.mode == Game.Mode.Test)
        {
            float dist = Vector2.Distance(Game.worldTouchPosition, new Vector2(transform.position.x, transform.position.y));

            if (placed)
            {
                selected = false;

            }
            else
            {

                if (!placed && !selected)
                {
                    if (dist < Game.Scale)
                    {
                        Game.SelectSquare(this);
                    }

                }

                if (selected)
                {
                   // transform.localScale = selectedScale;
                    transform.position = Game.worldTouchPosition;
                }else
                {
                   // transform.localScale = origScale;
                }
            }
        }


        UpdateColors();
        UpdateAbiltyIcon();

        //If Square is too far from the play area, delete it
        if (Mathf.Abs(transform.position.x) > 200f || Mathf.Abs(transform.position.y) > 200f)
        {
            Destroy(gameObject);
        }
    }



    void UpdateColors()
    {
        cellA.UpdateColor(aColor);
        cellB.UpdateColor(bColor);
        cellC.UpdateColor(cColor);
        cellD.UpdateColor(dColor); 
    }

    public void SetBlank()
    {
        cellA.UpdateColor(CellColor.wild);
        cellB.UpdateColor(CellColor.wild);
        cellC.UpdateColor(CellColor.wild);
        cellD.UpdateColor(CellColor.wild);
    }


    public bool PointInSquare(Vector2 point)
    {
        return (Vector2.Distance(point, Center) < radius);
        
    }

    public bool Matches(Square other, MatchDirection direction)
    {
        if (rotating)
        {
            return false;
        }

        switch (direction)
        {
            
            case MatchDirection.up:
                return (MatchCellColors(cellA, other.cellD, direction) && MatchCellColors(cellB, other.cellC, direction));

            case MatchDirection.down:
                return (MatchCellColors(cellD, other.cellA, direction) && MatchCellColors(cellC, other.cellB, direction));

            case MatchDirection.left:
                return (MatchCellColors(cellA, other.cellB, direction) && MatchCellColors(cellD, other.cellC, direction));

            case MatchDirection.right:
                return (MatchCellColors(cellB, other.cellA, direction) && MatchCellColors(cellC, other.cellD, direction));
        }
        return false;
    }
   
    public void Deselect()
    {
        if (!placed)
        {
            transform.position = origPosition;
        }
        selected = false;
    }


    public bool AffectNeighborsWithAbility()
    {
        bool abilityHasTriggered = false;
        List<Square> neighbors = Board.GetNeighborSquares(coords);
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] != null)
            {
                neighbors[i].ActivateAbilityOnSquare(type, ref abilityHasTriggered);

            }
        }

        return abilityHasTriggered;
    }

    public bool CheckNeighborAbilities()
    {
        bool abilityHasTriggered = false;
        List<Square> neighbors = Board.GetNeighborSquares(coords);
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] != null)
            {
                ActivateAbilityOnSquare(neighbors[i].type, ref abilityHasTriggered);

            }
        }
        return abilityHasTriggered;
    }


    public void ActivateAbilityOnSquare(SquareType type, ref bool flag)
    {
        switch (type)
        {
            case SquareType.normal:
                break;

            #region paint abilities:
            case SquareType.paint_red:
                PaintSquare(CellColor.red);
                flag = true;
                break;

            case SquareType.paint_blue:
                PaintSquare(CellColor.blue) ;
                flag = true;
                break;


            case SquareType.paint_cyan:
                PaintSquare(CellColor.cyan);
                flag = true;
                break;

            case SquareType.paint_orange:
                PaintSquare(CellColor.orange);
                flag = true;
                break;

            case SquareType.paint_green:
                PaintSquare(CellColor.green);
                flag = true;
                break;

            case SquareType.paint_purple:
                PaintSquare(CellColor.purple);
                flag = true;
                break;

            case SquareType.paint_dead:
                PaintSquare(CellColor.dead);
                flag = true;
                break;

            case SquareType.paint_wild:
                PaintSquare(CellColor.wild);
                flag = true;
                break;
            #endregion
            #region other abilities
            case SquareType.rotate_right:
                FWDRotate();
                flag = true;
                break;

            case SquareType.rotate_left:
                BWDRotate();
                flag = true;
                break;

            case SquareType.wind_up:
                Shift(MatchDirection.up);
                flag = true;
                break;

            case SquareType.wind_down:
                Shift(MatchDirection.down);
                flag = true;
                break;

            case SquareType.wind_left:
                Shift(MatchDirection.left);
                flag = true;
                break;

            case SquareType.wind_right:
                Shift(MatchDirection.right);
                flag = true;
                break;
                #endregion
        }
    }


    bool MatchCellColors(Cell cell1, Cell cell2, MatchDirection direction)
    {
        if(cell1.color == CellColor.dead || cell2.color == CellColor.dead)
        {
            return false;
        }

        if (cell1.color == cell2.color || cell1.color == CellColor.wild || cell2.color == CellColor.wild) {
            return true;
        }
        else
        {
            return false;
        }
    }
    
    public void UpdateData(Square_Data data)
    {

        aColor =  data.cellA_Color;
        cellA.UpdateColor(aColor);
        
        bColor =  data.cellB_Color;
        cellB.UpdateColor(bColor);


        cColor = data.cellC_Color;
        cellC.UpdateColor(cColor);


        dColor = data.cellD_Color;
        cellD.UpdateColor(dColor);

        coords = data.coords;
        typeSelection = data.type;
    }


    public void PaintSquare(CellColor color)
    {
        aColor = color;
        cellA.UpdateColor(color);

        bColor = color;
        cellB.UpdateColor(color);


        cColor = color;
        cellC.UpdateColor(color);


        dColor = color;
        cellD.UpdateColor(color);
    }


    public void Shift(MatchDirection moveDirection)
    {
        int x = 0, y = 0;

        switch (moveDirection)
        {
            case MatchDirection.up:
                x = 0;
                y = 1;
                break;

            case MatchDirection.down:
                x = 0;
                y = -1;
                break;

            case MatchDirection.right:
                x = 1;
                y = 0;
                break;

            case MatchDirection.left:
                x = -1;
                y = 0;
                break;
        }

        moveDelta = new Vector3(x, y, 0);
     
        this.moveDirection = moveDirection;
        if (!CheckShiftCollision(moveDirection))
        {
            Board.DislodgeSquare(this);
            shifting = true;
        }
    }

    private bool CheckShiftCollision(MatchDirection moveDirection)
    {
        List<Square> neighbors = Board.GetNeighborSquares(coords);

        Vector3 coordPos = Board.GetGridPosition(coords);
        int index = (int)moveDirection;

       if ( neighbors[index] != null && (Vector3.Distance(transform.position, coordPos) < 0.2f))
        {
            transform.position = coordPos;
            return true;
        }
        else
        {
            return false;
        }
    }


    public void FWDRotate()
    {
         foreach(Cell cell in Cells)
         {
            cell.LeftShift();
         }
        //Update the data

        Cell tempCell = cellA;
        cellA = cellD;
        cellD = cellC;
        cellC = cellB;
        cellB = tempCell;


        CellColor tempColor = aColor;
        aColor = dColor;
        dColor = cColor;
        cColor = bColor;
        bColor = tempColor;

        UpdateColors();
    }

    public void BWDRotate()
    {
        foreach(Cell cell in Cells)
         {
             cell.RightShift();
         }

        Cell tempCell = cellD;
        cellD = cellA;
        cellA = cellB;
        cellB = cellC;
        cellC = tempCell;

        CellColor tempColor = dColor;
        dColor = aColor;
        aColor = bColor;
        bColor = cColor;
        cColor = tempColor;
        
        UpdateColors();
    }

   

    
}


