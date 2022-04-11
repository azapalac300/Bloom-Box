using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareAbility : MonoBehaviour
{
    // Start is called before the first frame update
    public Square square;

    [SerializeField]
    private SquareType typeSelection;

    public SquareType Type { get; private set; }

    private GameObject icon;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       if(Type != typeSelection)
        {
            SetType(typeSelection);
        }
    }

    public void SetType(SquareType type)
    {
        Type = type;
        UpdateAbiltyIcon();
        typeSelection = type;
    }

    public void UpdateAbiltyIcon()
    {

        GameObject iconCandidate = null;
        switch (Type)
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

            case SquareType.locked:
                iconCandidate = Resources.Load<GameObject>("Lock");
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


    public void ActivateAbilityOnSquare(SquareType activatorType, ref bool flag)
    {
        if (Type == SquareType.locked)
        {
            return;
        }

        switch (activatorType)
        {
            case SquareType.normal:
                break;

            #region paint abilities:
            case SquareType.paint_red:
                square.PaintSquare(CellColor.red);
                flag = true;
                break;

            case SquareType.paint_blue:
                square.PaintSquare(CellColor.blue);
                flag = true;
                break;


            case SquareType.paint_cyan:
                square.PaintSquare(CellColor.cyan);
                flag = true;
                break;

            case SquareType.paint_orange:
                square.PaintSquare(CellColor.orange);
                flag = true;
                break;

            case SquareType.paint_green:
                square.PaintSquare(CellColor.green);
                flag = true;
                break;

            case SquareType.paint_purple:
                square.PaintSquare(CellColor.purple);
                flag = true;
                break;

            case SquareType.paint_dead:
                square.PaintSquare(CellColor.dead);
                flag = true;
                break;

            case SquareType.paint_wild:
                square.PaintSquare(CellColor.wild);
                flag = true;
                break;
            #endregion
            #region other abilities
            case SquareType.rotate_right:
                square.FWDRotate(true);
                flag = true;
                break;

            case SquareType.rotate_left:
                square.BWDRotate(true);
                flag = true;
                break;

            case SquareType.wind_up:
                square.Shift(MatchDirection.up);
                flag = true;
                break;

            case SquareType.wind_down:
                square.Shift(MatchDirection.down);
                flag = true;
                break;

            case SquareType.wind_left:
                square.Shift(MatchDirection.left);
                flag = true;
                break;

            case SquareType.wind_right:
                square.Shift(MatchDirection.right);
                flag = true;
                break;

            case SquareType.locked:
                break;
                #endregion
        }
    }


    public bool CheckNeighborAbilities()
    {
        bool abilityHasTriggered = false;
        List<Square> neighbors = Board.GetNeighborSquares(square.coords);
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] != null)
            {

                ActivateAbilityOnSquare(neighbors[i].ability.Type, ref abilityHasTriggered);

            }
        }
        return abilityHasTriggered;
    }

    public bool AffectNeighborsWithAbility()
    {
        bool abilityHasTriggered = false;
        List<Square> neighbors = Board.GetNeighborSquares(square.coords);
        for (int i = 0; i < neighbors.Count; i++)
        {
            if (neighbors[i] != null)
            {
                neighbors[i].ability.ActivateAbilityOnSquare(Type, ref abilityHasTriggered);

            }
        }

        return abilityHasTriggered;
    }

}
