using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


#if (UNITY_EDITOR)

public class MakeLevelData{
    [MenuItem("Assets/Create/Level Data File")]
    public static LevelDataAsset CreateFile(int levelNum, Level_Data data )
    {
        LevelDataAsset a = ScriptableObject.CreateInstance<LevelDataAsset>();
        a.data = data;
        AssetDatabase.CreateAsset(a, "Assets/Resources/LevelFiles/Level" + levelNum + "data.asset");
        EditorUtility.SetDirty(a);
        AssetDatabase.SaveAssets();
        
        return a;
    }


 }
#endif