using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class LevelCollections
{
    public static string[] Level1 = new string[] { "Level_1A", "Level_1B" };
    public static string[] Level2 = new string[] { "Level_2A", "Level_2B" };

    public static bool CheckSceneInCollection(string sceneName)
    {
        if (Level1.Contains(sceneName) || Level2.Contains(sceneName)) return true;
        else return false;
    }

    public static string[] GetCollectionNameFromScene(string sceneName)
    {
        if (Level1.Contains(sceneName)) return Level1;
        else if (Level2.Contains(sceneName)) return Level2;
        else return null;
    }
}
