using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if (UNITY_EDITOR)

public class MakeLevelData{


    public static LevelDataAsset MakePuzzleLevel(int levelNum, Level_Data data)
    {
        return CreateFile(levelNum, data, "Level");
    }

    public static LevelDataAsset MakeEndlessLevel(int levelNum, Level_Data data)
    {
        return CreateFile(levelNum, data, "EndlessLevel");
    }


    public static LevelDataAsset CreateFile(int levelNum, Level_Data data, string levelType)
    {
        LevelDataAsset a = ScriptableObject.CreateInstance<LevelDataAsset>();
        a.data = data;
        AssetDatabase.CreateAsset(a, "Assets/Resources/LevelFiles/" + levelType + levelNum + "data.asset");
        EditorUtility.SetDirty(a);
        AssetDatabase.SaveAssets();
        
        return a;
    }


 }
#endif