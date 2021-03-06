using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof (SpriteRenderer))][ExecuteInEditMode]
public class Cell : MonoBehaviour {
    public bool LeftShifting { get; private set; }
    public bool RightShifting { get; private set; }
    SpriteRenderer spriteRenderer;
    public bool debug;
    public Position position;
    public CellColor color;
    private CellColor prevColor;

    Square parentSquare;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Use this for initialization
    void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
        
        if (LeftShifting)
        {
            LeftShifting = Shift(1, LeftShifting);
        }

        if (RightShifting)
        {
            RightShifting = Shift(-1, RightShifting);
        }

       
        //spriteRenderer.color = GetColor(color);

        if(prevColor != color)
        {
            UpdateColor(color);
            prevColor = color;
        }
        
    }
    
    public void SetUp(Square square, Position startPosition)
    {
        position = startPosition;
        parentSquare = square;
        LeftShifting = false;
        RightShifting = false;
        transform.Translate((Vector2)transform.position - square.Center + PositionVector);
        UpdateColor(color);
    }

    public void LeftShift() {
        LeftShifting = true;
    }

    public void RightShift()
    {
        RightShifting = true;
    }

    private bool Shift(int direction, bool shiftFlag)
    {
        Vector2 targetVector = Vector2.zero;

        if(direction > 0)
        {
            targetVector = NextPositionVector*parentSquare.currentScale;
        }
        else
        {
            targetVector = PrevPositionVector*parentSquare.currentScale;
        }

        Vector3 diff = (Vector3)(parentSquare.Center + targetVector) - transform.position;
        diff.Normalize();
        transform.Translate(diff * Game.RotateSpeed * Time.deltaTime);
        float dist = Vector2.Distance(transform.position, parentSquare.Center + targetVector);

        if (dist < Game.RotateSpeed * Time.deltaTime)
        {
            shiftFlag = false;
            transform.Translate((Vector3)(parentSquare.Center + targetVector) - transform.position);

            if (direction > 0)
            {
                position = NextPosition;
            }
            else
            {
                position = PrevPosition;
            }
        }

        return shiftFlag;
    }
    


    public void UpdateColor(CellColor cellColor)
    {
       
        UpdateSprite();
        color = cellColor;


        
        spriteRenderer.color = SelectColor(cellColor);
    }


    public static Color SelectColor(CellColor cellColor)
    {
        Color c = Color.white;

        switch (cellColor)
        {
            //Wild and Dead both do something special when the sprite is updated

            case CellColor.red:

                ColorUtility.TryParseHtmlString("#ff3300", out c);
                break;

            case CellColor.white:
                ColorUtility.TryParseHtmlString("#ffffff", out c);
                break;

            case CellColor.orange:
                ColorUtility.TryParseHtmlString("#fdad3f", out c);
                break;


            case CellColor.cyan:
                ColorUtility.TryParseHtmlString("#00E3E5", out c);
                break;


            case CellColor.green:
                ColorUtility.TryParseHtmlString("#00CB00", out c);
                break;

            case CellColor.blue:
                ColorUtility.TryParseHtmlString("#1A68FF", out c);
                break;

            case CellColor.purple:
                ColorUtility.TryParseHtmlString("#dea3ff", out c);
                break;

            case CellColor.dead:
                ColorUtility.TryParseHtmlString("#7B7B7B", out c);
                break;

        }
        return c;
    }

    #region Position enum stuff

    Vector2 PrevPositionVector
    {
        get
        {
            switch (position)
            {
                case Position.A:
                    return positionD;

                case Position.D:
                    return positionC;

                case Position.C:
                    return positionB;

                case Position.B:
                    return positionA;

               

                default:
                    Debug.LogError("Something went wrong!");
                    return new Vector2();

            }
        }
    }

    Vector2 NextPositionVector
    {
        get
        {
            switch (position)
            {
                case Position.A:
                    return positionB;

                case Position.B:
                    return positionC;

                case Position.C:
                    return positionD;

                case Position.D:
                    return positionA;

                default:
                    Debug.LogError("Something went wrong!");
                    return new Vector2();

            }
        }
    }

    Position PrevPosition
    {
        get
        {
            switch (position)
            {
                case Position.A:
                    return Position.D;

                case Position.D:
                    return Position.C;

                case Position.C:
                    return Position.B;

                case Position.B:
                    return Position.A;



                default:
                    Debug.LogError("Something went wrong!");
                    return Position.A;

            }
        }
    }

    Position NextPosition
    {
        get
        {
            switch (position)
            {
                case Position.A:
                    return Position.B;

                case Position.B:
                    return Position.C;

                case Position.C:
                    return Position.D;

                case Position.D:
                    return Position.A;

                default:
                    Debug.LogError("Something went wrong!");
                    return Position.A;

            }
        }
    }

    Vector2 PositionVector
    {
        get
        {
            switch (position)
            {
                case Position.A:
                    return positionA;

                case Position.B:
                    return positionB;

                case Position.C:
                    return positionC;

                case Position.D:
                    return positionD;

                default:
                    return new Vector2();

            }
        }
    }

    public void UpdateSprite()
    {
        string spriteName = "Petal";

        if (color == CellColor.wild)
        {
            spriteName += "_Rainbow-";
        }
        else if (color == CellColor.dead)
        {
            spriteName += "_Dead-";
        }
        else
        {
            spriteName += "_White-";
        }

        switch (position)
        {
            case Position.A:
                spriteName += "Top-Left";
                break;
            case Position.B:
                spriteName += "Top-Right";
                break;
            case Position.C:
                spriteName += "Bottom-Right";
                break;
            case Position.D:
                spriteName += "Bottom-Left";
                break;
        }



       //Debug.Log(spriteName);
       Sprite sprite = Resources.Load<Sprite>("Petals/" + spriteName);
       GetComponent<SpriteRenderer>().sprite = sprite;

    }


    public void PaintCell()
    {



    }

    public static CellColor CellPaintPriority(Cell cell1, Cell cell2, Cell cell3)
    {
        CellColor cellColor = cell1.color;
        if (cell2.color == cell3.color)
        {
            return cell2.color;
        }

        if (cell2.color == CellColor.white)
        {
            return cell3.color;
        }

        if (cell3.color == CellColor.white)
        {
            return cell2.color;
        }

        if (cell2.color == CellColor.wild)
        {
            return cell2.color;
        }

        if (cell3.color == CellColor.wild)
        {
            return cell3.color;
        }

        return cell1.color;
    }

    Vector2 positionA { get { return new Vector2(-Game.Scale / 2, Game.Scale / 2); } }
    Vector2 positionB { get { return new Vector2(Game.Scale/2, Game.Scale/2) ; } }
    Vector2 positionC { get { return new Vector2(Game.Scale/2, -Game.Scale/2) ; } }
    Vector2 positionD { get { return new Vector2(-Game.Scale/2, -Game.Scale/2)  ; } }

    #endregion


    public MatchDirection[] GetMatchDirections()
    {
        MatchDirection[] directions = new MatchDirection[] {MatchDirection.count, MatchDirection.count};

        switch (position)
        {
            case Position.A:
                directions[0] = MatchDirection.left;
                directions[1] = MatchDirection.up;
                break;

            case Position.B:
                directions[0] = MatchDirection.up;
                directions[1] = MatchDirection.right;
                break;

            case Position.C:
                directions[0] = MatchDirection.right;
                directions[1] = MatchDirection.down;
                break;

            case Position.D:
                directions[0] = MatchDirection.down;
                directions[1] = MatchDirection.left;
                break;

        }


        return directions;
    }

    public CellOrder GetMatchingCell(MatchDirection direction)
    {
        CellOrder order = CellOrder.cellA;
        
        switch (position)
        {
            
            case Position.A:
                if(direction == MatchDirection.left)
                {
                    return CellOrder.cellB;
                }

                if (direction == MatchDirection.up)
                {
                    return CellOrder.cellD;
                }
                break;

            case Position.B:
                if (direction == MatchDirection.up)
                {
                    return CellOrder.cellC;
                }

                if (direction == MatchDirection.right)
                {
                    return CellOrder.cellA;
                }
                break;

            case Position.C:
                if (direction == MatchDirection.right)
                {
                    return CellOrder.cellD;
                }

                if (direction == MatchDirection.down)
                {
                    return CellOrder.cellB;
                }
                break;

            case Position.D:
                if (direction == MatchDirection.down)
                {
                    return CellOrder.cellA;
                }

                if (direction == MatchDirection.left)
                {
                    return CellOrder.cellC;
                }
                break;
        }

        return order;
    }

}

