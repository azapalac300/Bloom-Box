using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlaySettings : ScriptableObject
{
    public int currentLevel;
    public bool loadedFromMenu;
    public bool testMode;
}
