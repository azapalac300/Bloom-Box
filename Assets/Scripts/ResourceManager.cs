using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager
{
    public static Vector2 Flatten(Vector3 vect)
    {
        return new Vector2(vect.x, vect.y);
    }

  
}

public enum CellColor
{
    wild, red, blue, cyan, orange, green, purple, dead
}

public enum MatchDirection
{
    right = 0,
    up,
    left,
    down
}

public enum SquareType
{
    normal, rotate_right, rotate_left, wind_up, wind_down, wind_left, wind_right,
    paint_wild, paint_red, paint_blue, paint_cyan, paint_orange, paint_green, paint_purple, paint_dead
}

public enum Position
{
    A, B, C, D
}


