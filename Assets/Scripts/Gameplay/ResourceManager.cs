using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public static Vector2 Flatten(Vector3 vect)
    {
        return new Vector2(vect.x, vect.y);
    }


    public static float minSwipeDist = 250f;
    public static float diagonalTolerance = 100f;
    public static float selectedScaleFactor = 1.2f;
    public static float highlightedScaleFactor = 1.2f;
    public static float squareFollowDist = 3f;


    public static CellColor AddCellColors(CellColor a, CellColor b)
    {
        if(a == CellColor.dead)
        {
            return a;
        }

        if(b == CellColor.dead)
        {
            return b;
        }

        if(a == CellColor.white)
        {
            return b;
        }


        if(b == CellColor.white)
        {
            return a;
        }

        if(a == b)
        {
            return a;
        }

        return CellColor.wild;
    }

    public static Vector2 AddShiftDir(ShiftDirection a, ShiftDirection b)
    {

        return (GetShiftVect(a) + GetShiftVect(b));
    }


    private static Vector2 GetShiftVect( ShiftDirection s)
    {
        switch (s)
        {
            case ShiftDirection.none:
                return new Vector2(0, 0);

            case ShiftDirection.up:
                return new Vector2(0, 1);

            case ShiftDirection.down:
                return new Vector2(0, -1);

            case ShiftDirection.right:
                 return new Vector2(1, 0);

            case ShiftDirection.left:
                return new Vector2(-1, 0);

        }

        return new Vector2(0, 0);
    }

}

public enum CellColor
{
    wild, red, blue, cyan, orange, green, purple, dead, white
}

public enum MatchDirection
{
    right = 0,
    up,
    left,
    down,
    count
}

public enum diagMatchDirection
{
    upRight = 0,
    upLeft, 
    downRight,
    downLeft,
    count
}


public enum ShiftDirection
{
    none = 0,
    right,
    up,
    left,
    down,
    upRight,
    upLeft,
    downRight,
    downLeft
}

public enum CellOrder
{
    cellA = 0,
    cellB,
    cellC,
    cellD
}

public enum SquareType
{
    normal, rotate_right, rotate_left, wind_up, wind_down, wind_left, wind_right,
    paint_wild, paint_red, paint_blue, paint_cyan, paint_orange, paint_green, paint_purple, paint_dead, paint_white, locked
}

/*
public enum SquareTypeCategory
{
    Normal, Rotate, Wind, Paint, Locked
}
*/

public enum RotateDirection
{
    FWD, BWD
}

public enum Position
{
    A, B, C, D
}


