using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu]
public class Levels : ScriptableObject
{
    public void AddLevel(Level_Data data)
    {

        if (list == null)
        {
            list = new List<LevelDataAsset>();
        }

#if(UNITY_EDITOR)

        LevelDataAsset a = MakeLevelData.CreateFile(list.Count, data);

        list.Add(a);

        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
#endif

    }

    public void SaveLevel(int levelNum, Level_Data data)
    {
        if (levelNum >= list.Count)
        {
            AddLevel(data);
            return;
        }

        if (levelNum >= 0)
        {
#if (UNITY_EDITOR)
            list[levelNum].data = data;
            EditorUtility.SetDirty(list[levelNum]);
            AssetDatabase.SaveAssets();
#endif
        }

        
    }


    public Level_Data LoadLevel(int levelNum)
    {
        if (levelNum <= list.Count - 1)
        {
            return list[levelNum].data;
        }
        else
        {
            return new Level_Data();
        }
    }

    public List<LevelDataAsset> list;
}

#region serializable classes
[System.Serializable]
public class Vector3_Data
{
    public Vector3 testVector;
    public float x, y, z;

    public static Vector3_Data toData(Vector3 v)
    {
        Vector3_Data d = new Vector3_Data();
        d.x = v.x;
        d.y = v.y;
        d.z = v.z;
        return d;
    }

    public Vector3 toVector3()
    {
        return new Vector3(x, y, z);
    }

}


[System.Serializable]
public class Square_Data
{
    public Vector3_Data position;

    public CellColor cellA_Color, cellB_Color, cellC_Color, cellD_Color;

    public SquareType type;

    public int[] coords;

    public Square_Data()
    {
        coords = new int[2];
    }

    public static Square_Data toData(Square s)
    {
        Square_Data sd = new Square_Data();
        sd.position = Vector3_Data.toData(s.transform.position);
        sd.type = s.type;
        sd.cellA_Color = s.cellA.color;
        sd.cellB_Color = s.cellB.color;
        sd.cellC_Color = s.cellC.color;
        sd.cellD_Color = s.cellD.color;
        sd.coords = s.coords;
        return sd;
    }
}


[System.Serializable]
public class IntPair
{
    public int x;
    public int y;
    public IntPair(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

}

[System.Serializable]
public class Board_Data
{
    [SerializeField]
    public List<IntPair> goalSquareCoords;

    public List<Square_Data> boardSquares;

    public Board_Data()
    {
        goalSquareCoords = new List<IntPair>();
        boardSquares = new List<Square_Data>();
    }
    public static Board_Data toData(Board b)
    {
        Board_Data bd = new Board_Data();


        for (int i = 0; i < b.goalMarkers.Count; i++)
        {
            bd.goalSquareCoords.Add(b.GoalMarkerCoords(i));
        }

        foreach (KeyValuePair<string, Square> pair in Board.placedSquares)
        {
            bd.boardSquares.Add(Square_Data.toData(pair.Value));
        }

        return bd;
    }

}


[System.Serializable]
public class Hand_Data
{
    public List<Square_Data> handSquares;

    public Hand_Data()
    {

        handSquares = new List<Square_Data>();
    }

    public static Hand_Data toData(Hand h)
    {
        Hand_Data hd = new Hand_Data();
        for (int i = 0; i < h.squaresInHand.Count; i++)
        {
            hd.handSquares.Add(Square_Data.toData(h.squaresInHand[i]));
        }

        return hd;
    }
}


[System.Serializable]
public class Level_Data
{

    public Board_Data boardData;
    public Hand_Data handData;
    public string tutorialData;

    public Level_Data()
    {
        boardData = new Board_Data();
        handData = new Hand_Data();
    }

}


#endregion