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
    paint_wild, paint_red, paint_blue, paint_cyan, paint_orange, paint_green, paint_purple, paint_dead, paint_white, framed
}

public enum Position
{
    A, B, C, D
}


