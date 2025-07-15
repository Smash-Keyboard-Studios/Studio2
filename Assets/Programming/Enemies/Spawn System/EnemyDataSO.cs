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
// © 2025 Dominic McNeill dommcneill@outlook.com
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.

/// <summary>
/// Used to store data in the aiSpawnList, as dictionaries cannot serialize.
/// </summary>
[Serializable]
public struct Enemy
{
    /// <summary>
    /// The id for the associated enemy prefab.
    /// </summary>
    public int id;

    /// <summary>
    /// The enemy prefab associated with the id.
    /// </summary>
    public GameObject prefab;
}

/// <summary>
/// This is used to store data of all enemy prefabs and associate an id number with them.
/// </summary>
[CreateAssetMenu(fileName = "EnemyData", menuName = "EnemyData/SpawnWaveCoreData")]
public class EnemyDataSO : ScriptableObject
{
    /// <summary>
    /// The list of all enemies in the game that can spawn.
    /// </summary>
    [Header("AI spawn list"), SerializeField]
    public Enemy[] aISpawnList;
}
