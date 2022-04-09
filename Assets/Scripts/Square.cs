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
    private Vector3 highlightedScale;
    private Vector3 origScale;

    public float currentScale { get; private set; }

    private GameObject icon;

    private Vector2 offset;

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
        currentScale = 1f;
        SetUpCells();
        UpdateAbiltyIcon();

        TouchControls.SwipeDownEvent += FWDRotate;
        TouchControls.SwipeUpEvent += BWDRotate;
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

            case SquareType.framed:
                iconCandidate = Resources.Load<GameObject>("Frame");
                break;

                #endregion
        }

        if (iconCandidate != null)
        {
            icon = Instantiate(iconCandidate, transform.position + new Vector3(0, 0, -1f), Quaternion.identity);
            icon.transform.localScale *= Game.Scale;
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
            selectedScale = origScale * ResourceManager.selectedScaleFactor;
            highlightedScale = selectedScale * ResourceManager.highlightedScaleFactor;

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
        cell.transform.parent = transform;
        cell.SetUp(this, position);

        
        return cell;
    }

    // Update is called once per frame
    void Update() {


        coords = Board.GetGridCoordinates(transform.position);

        transform.localScale = origScale;
        currentScale = 1f;

        if (selected)
        {
            transform.localScale = selectedScale;
            currentScale = ResourceManager.selectedScaleFactor;
        }
       
        

        if (highlighted)
        {
            transform.localScale = highlightedScale;
            currentScale = ResourceManager.highlightedScaleFactor;
        }
      
        

        //Can be shortened because all cells shift at once
        tray = inTray;

        if (Input.GetKeyDown(KeyCode.R) )
        {
            FWDRotate();
        }


        if (Input.GetKeyDown(KeyCode.Q))
        {
            BWDRotate();
        }

        rotating = (cellD.LeftShifting || cellD.RightShifting);

        if (shifting)
        {
            transform.Translate(moveDelta * Game.ShiftSpeed * Time.deltaTime);
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
                   Debug.DrawLine(Game.worldTouchPosition, new Vector2(transform.position.x, transform.position.y));

                    if (dist < Game.Scale)
                    {
                        Game.SelectSquare(this);
                    }

                }

                if (selected)
                {

                    transform.position = Game.worldTouchPosition + new Vector2(0, ResourceManager.squareFollowDist);
                    float followDist = Vector3.Distance(Game.worldTouchPosition, flatPosition);

                    if (Vector3.Distance(Game.worldTouchPosition, flatPosition) >= ResourceManager.squareFollowDist)
                    {
                       
                        
                    }
                    else
                    {
                        offset = flatPosition - Game.worldTouchPosition;
                    }
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


    public bool Matches(List<Square> neighbors)
    {

        if (rotating)
        {
            return false;
        }

       bool flag = true;
        for (int i = 0; i < Cells.Count; i++)
        {
            CellOrder targetCell = (CellOrder)i;
            switch (targetCell)
            {
                #region CellA
                case (CellOrder.cellA):

                    if (neighbors[(int)MatchDirection.up] == null &&
                        (neighbors[(int)MatchDirection.left] == null))
                    {
                        break;
                    }

                    if (neighbors[(int)MatchDirection.up] == null)
                    {
                        flag = flag && MatchCells(cellA, neighbors[(int)MatchDirection.left].cellB);
                        break;
                    }

                    if (neighbors[(int)MatchDirection.left] == null)
                    {
                        flag = flag && MatchCells(cellA, neighbors[(int)MatchDirection.up].cellD);
                        break;
                    }

                    flag = flag && MatchCells(cellA, neighbors[(int)MatchDirection.left].cellB, neighbors[(int)MatchDirection.up].cellD);


                    break;
                #endregion

                #region CellB
                case (CellOrder.cellB):

                    if (neighbors[(int)MatchDirection.up] == null &&
                        (neighbors[(int)MatchDirection.right] == null))
                    {
                        break;
                    }

                    if (neighbors[(int)MatchDirection.up] == null)
                    {
                        flag = flag && MatchCells(cellB, neighbors[(int)MatchDirection.right].cellA);
                        break;
                    }

                    if (neighbors[(int)MatchDirection.right] == null)
                    {
                        flag = flag && MatchCells(cellB, neighbors[(int)MatchDirection.up].cellC);
                        break;
                    }

                    flag = flag && MatchCells(cellB, neighbors[(int)MatchDirection.right].cellA, neighbors[(int)MatchDirection.up].cellC);
                    if (flag)
                    {
                        Debug.Log("Cell B flag true!");
                    }
                    break;
                #endregion

                #region CellC
                case (CellOrder.cellC):

                    if (neighbors[(int)MatchDirection.down] == null &&
                        (neighbors[(int)MatchDirection.right] == null))
                    {
                        break;
                    }

                    if (neighbors[(int)MatchDirection.down] == null)
                    {
                        flag = flag && MatchCells(cellC, neighbors[(int)MatchDirection.right].cellD);
                        break;
                    }

                    if (neighbors[(int)MatchDirection.right] == null)
                    {
                        flag = flag && MatchCells(cellC, neighbors[(int)MatchDirection.down].cellB);
                        break;
                    }

                    flag = flag && MatchCells(cellC, neighbors[(int)MatchDirection.right].cellD, neighbors[(int)MatchDirection.down].cellB);
                    break;
                #endregion

                #region CellD
                case (CellOrder.cellD):

                    if (neighbors[(int)MatchDirection.down] == null &&
                        (neighbors[(int)MatchDirection.left] == null))
                    {
                        break;
                    }

                    if (neighbors[(int)MatchDirection.down] == null)
                    {
                        flag = flag && MatchCells(cellD, neighbors[(int)MatchDirection.left].cellC);
                        break;
                    }

                    if (neighbors[(int)MatchDirection.left] == null)
                    {
                        flag = flag && MatchCells(cellD, neighbors[(int)MatchDirection.down].cellA);
                        break;
                    }

                    flag = flag && MatchCells(cellD, neighbors[(int)MatchDirection.left].cellC, neighbors[(int)MatchDirection.down].cellA);
                    break;
                    #endregion


            } 
        }

        return flag;
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


    public void ActivateAbilityOnSquare(SquareType activatorType, ref bool flag)
    {
        if(type == SquareType.framed)
        {
            return;
        }

        switch (activatorType)
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
                FWDRotate(true);
                flag = true;
                break;

            case SquareType.rotate_left:
                BWDRotate(true);
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

            case SquareType.framed:
                break;
                #endregion
        }
    }

    public void PaintWhiteCells(Square newlyPlacedSquare, MatchDirection direction)
    {

        switch (direction)
        {

            case MatchDirection.down:
                if (cellD.color == CellColor.white) { 
                    UpdateCellColor(cellA, ref aColor, newlyPlacedSquare.cellD.color); 
                }
                if (cellB.color == CellColor.white) { 
                    UpdateCellColor(cellB, ref bColor, newlyPlacedSquare.cellC.color); 
                }
                break;

            case MatchDirection.up:
                if (cellD.color == CellColor.white) { 
                    UpdateCellColor(cellD, ref dColor, newlyPlacedSquare.cellA.color);
                }
                if (cellC.color == CellColor.white) {
                    UpdateCellColor(cellC, ref cColor, newlyPlacedSquare.cellB.color);
                }
                break;

            case MatchDirection.left:
                if (cellB.color == CellColor.white) {
                    UpdateCellColor(cellB, ref bColor, newlyPlacedSquare.cellA.color); 
                }
                if (cellC.color == CellColor.white) {
                    UpdateCellColor(cellC, ref cColor, newlyPlacedSquare.cellD.color); 
                }
                break;

            case MatchDirection.right:
                if (cellA.color == CellColor.white) {
                    UpdateCellColor(cellA, ref aColor, newlyPlacedSquare.cellB.color);
                }
                if (cellD.color == CellColor.white) {
                    UpdateCellColor(cellD, ref dColor, newlyPlacedSquare.cellD.color);
                }
                break;
        }
    }

    public void PaintWhiteCells(List<Square> neighbors)
    {
        for (int i = 0; i < Cells.Count; i++)
        {
            CellOrder targetCell = (CellOrder)i;

            if (Cells[i].color == CellColor.white)
            {
                switch (targetCell)
                {
                    #region CellA
                    case CellOrder.cellA:

                        if(neighbors[(int)MatchDirection.up] == null && neighbors[(int)MatchDirection.left] == null)
                        {
                            break;
                        }

                        if (neighbors[(int)MatchDirection.up] == null)
                        {
                            UpdateCellColor(cellA, ref aColor, neighbors[(int)MatchDirection.left].cellB.color);
                            break;
                        }

                        if (neighbors[(int)MatchDirection.left] == null)
                        {
                            UpdateCellColor(cellA, ref aColor, neighbors[(int)MatchDirection.up].cellD.color);
                            break;
                        }

                        CellColor newAColor = Cell.CellPaintPriority(cellA, neighbors[(int)MatchDirection.up].cellD, neighbors[(int)MatchDirection.left].cellB);
                        UpdateCellColor(cellA, ref aColor, newAColor);
                        break;
                    #endregion

                    #region CellB
                    case CellOrder.cellB:

                        if (neighbors[(int)MatchDirection.up] == null && neighbors[(int)MatchDirection.right] == null)
                        {
                            break;
                        }

                        if (neighbors[(int)MatchDirection.up] == null)
                        {
                            UpdateCellColor(cellB, ref bColor, neighbors[(int)MatchDirection.right].cellA.color);
                            break;
                        }

                        if (neighbors[(int)MatchDirection.right] == null)
                        {
                            UpdateCellColor(cellB, ref bColor, neighbors[(int)MatchDirection.up].cellC.color);
                            break;
                        }

                        CellColor newBColor = Cell.CellPaintPriority(cellB, neighbors[(int)MatchDirection.up].cellC, neighbors[(int)MatchDirection.right].cellA);
                        UpdateCellColor(cellB, ref bColor, newBColor);
                        break;
                    #endregion

                    #region CellC
                    case CellOrder.cellC:

                        if (neighbors[(int)MatchDirection.down] == null && neighbors[(int)MatchDirection.right] == null)
                        {
                            break;
                        }

                        if (neighbors[(int)MatchDirection.down] == null)
                        {
                            UpdateCellColor(cellC, ref cColor, neighbors[(int)MatchDirection.right].cellD.color);
                            break;
                        }

                        if (neighbors[(int)MatchDirection.right] == null)
                        {
                            UpdateCellColor(cellC, ref cColor, neighbors[(int)MatchDirection.down].cellB.color);
                            break;
                        }

                        CellColor newCColor = Cell.CellPaintPriority(cellC, neighbors[(int)MatchDirection.down].cellB, neighbors[(int)MatchDirection.right].cellD);
                        UpdateCellColor(cellC, ref cColor, newCColor);
                        break;
                    #endregion

                    #region CellD
                    case CellOrder.cellD:

                        if (neighbors[(int)MatchDirection.down] == null && neighbors[(int)MatchDirection.left] == null)
                        {
                            break;
                        }

                        if (neighbors[(int)MatchDirection.down] == null)
                        {
                            UpdateCellColor(cellD, ref dColor, neighbors[(int)MatchDirection.left].cellC.color);
                            break;
                        }

                        if (neighbors[(int)MatchDirection.left] == null)
                        {
                            UpdateCellColor(cellD, ref dColor, neighbors[(int)MatchDirection.down].cellA.color);
                            break;
                        }

                        CellColor targetColor = Cell.CellPaintPriority(cellD, neighbors[(int)MatchDirection.down].cellA, neighbors[(int)MatchDirection.left].cellC);
                        UpdateCellColor(cellD, ref dColor, targetColor);
                        break;
                        #endregion
                }
            }
        }

    }

    private void UpdateCellColor(Cell cell, ref CellColor colorToSet, CellColor targetColor)
    {
        colorToSet = targetColor;
        cell.UpdateColor(colorToSet);
    }

    
    public Square GetNeighbor(List<Square> neighbors, MatchDirection direction)
    {
        return neighbors[(int)direction];
    }

    bool MatchCells(Cell cell1, Cell cell2, Cell cell3)
    {
        if(cell1.color == CellColor.wild)
        {
            return true;
        }

        if (cell1.color == CellColor.dead || cell2.color == CellColor.dead || cell3.color == CellColor.dead)
        {
            return false;
        }

        bool b1 = MatchCells(cell1, cell2);

        bool b2 = MatchCells(cell2, cell3);

        bool b3 = MatchCells(cell2, cell3);

        if (b1 && b2 && b3)
        {
            int i = 0;
        }

         return (b1 && b2 && b3);
    }

    bool MatchCells(Cell cell1, Cell cell2)
    {

        if (cell1.color == cell2.color || cell1.color == CellColor.wild || cell2.color == CellColor.wild
            || cell1.color == CellColor.white || cell2.color == CellColor.white) {
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
        FWDRotate(false);
    }
    public void FWDRotate(bool bypass)
    {
        if (type == SquareType.framed)
        {
            return;
        }

        if ((!rotating && selected) || bypass)
        {
           
            foreach (Cell cell in Cells)
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
    }
    public void BWDRotate()
    {
        BWDRotate(false);
    }
    public void BWDRotate(bool bypass)
    {
        if(type == SquareType.framed)
        {
            return;
        }

        if ((!rotating && selected)|| bypass)
        {
            
            foreach (Cell cell in Cells)
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

}


