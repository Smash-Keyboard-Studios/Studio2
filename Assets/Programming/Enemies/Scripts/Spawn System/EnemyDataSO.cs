using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
