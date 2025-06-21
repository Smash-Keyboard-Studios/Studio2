using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


/// <summary>
/// Used to store data in the aiSpawnList, as dictionaries cannot serialize.
/// </summary>
[Serializable]
public struct Enemy
{
    public int id;
    public GameObject prefab;
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/SpawnWaveCoreData")]
public class EnemyDataSO : ScriptableObject
{
    [Header("AI spawn list"), SerializeField]
    public Enemy[] aISpawnList;
}
