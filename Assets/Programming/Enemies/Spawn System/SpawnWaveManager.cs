using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|
// © 2025 Dominic McNeill dommcneill@outlook.com

/// <summary>
/// Alternative to using list or dictionary for spawn parameters.
/// </summary>
[Serializable]
public struct EnemiesInWaveData
{
    public int id;
    public int amount;
}

[Serializable]
public struct WaveData
{
    public float delayBeforeWaveStart;
    public EnemiesInWaveData[] EnemiesInWave;
}

public class SpawnWaveManager : MonoBehaviour
{
    public EnemyDataSO enemyCoreData;

    public WaveData[] waveData;

    public List<GameObject> spawnPoints;



    public UnityEvent onWaveEnd;

    public UnityEvent onAllWavesCleared;

    public float minSpawnRadius = 15;
    public float maxSpawnRadius = 40;


    private List<Transform> trackingEnemies = new List<Transform>();

    private int enemyCount = 0;

    private Transform playerTransform;


    private Coroutine waveCoroutine;


    void Awake()
    {
        playerTransform = GameObject.Find("Player").transform;

        if (playerTransform == null)
        {
            Debug.LogError("Cannot locate the player and get transform!");
        }
    }

    void Update()
    {
        //print(enemyCount);

        if (playerTransform == null)
        {
            Debug.LogError("Cannot locate the player and get transform! Trying again.");
            playerTransform = GameObject.Find("Player").transform;
        }
    }


    public void SpawnTheWave()
    {
        if (waveCoroutine != null) return;

        waveCoroutine = StartCoroutine(ManageSpawningTheWave());
    }


    private IEnumerator ManageSpawningTheWave()
    {
        List<int> waveDataAsIDs = new List<int>();

        foreach (var wave in waveData)
        {
            waveDataAsIDs = ConvertEnemiesInWaveToIDList(wave.EnemiesInWave);

            yield return new WaitForSeconds(wave.delayBeforeWaveStart);

            RandomiseList(ref waveDataAsIDs);

            yield return null; // wait a frame.

            for (int i = 0; i < waveDataAsIDs.Count; i++)
            {
                SpawnAI(waveDataAsIDs[i], spawnPoints, playerTransform.position, minSpawnRadius, maxSpawnRadius);
                yield return null;
            }

            waveDataAsIDs.Clear();

            while (enemyCount > 0)
            {
                yield return null;
            }

            print("Wave was cleared");
            onWaveEnd?.Invoke();
        }

        print("All eaves were cleared");
        onAllWavesCleared?.Invoke();
    }


    #region SpawnAI
    /// <summary>
    /// Spawn the AI with the given id, min radius to spawn and maxRadius.
    /// </summary>
    /// <param name="id">The id of the ai to look for in aiSpawnList.</param>
    /// <param name="minRadius">How close the AI can spawn.</param>
    /// <param name="maxRadius">How far the AI can spawn.</param>
    /// <returns>True if spawning was successful.</returns>
    private bool SpawnAI(int id, List<GameObject> spawnLocationToUse, Vector3 playerPosition, float minRadius, float maxRadius)
    {
        // get the AI prefab from the id given.
        GameObject aiPrefab = GetEnemyPrefabFromID(id);

        if (aiPrefab == null)
        {
            Debug.LogWarning($"Prefab missing for ai id of {id}");
            return false;
        }

        // get a random spawn location that is suitable.
        Vector3? spawnLocation = GetRandomSpawnLocation(spawnLocationToUse, playerPosition, minRadius, maxRadius);

        if (!spawnLocation.HasValue)
        {
            Debug.LogWarning($"Cannot find a suitable location to spawn the AI");
            return false;
        }

        // spawn the AI and set state to alerted. This is not proc gen. this is wave gen.
        GameObject ai = Instantiate(aiPrefab, spawnLocation.Value, Quaternion.identity, null);
        ai.GetComponent<AIBase>().ChangeState(AIState.Alerted);
        ai.GetComponent<AIBase>().onDeath += RemoveEnemy;

        AddEnemy(ai.transform);

        return true;
    }
    #endregion


    #region GetRandomSpawnLocation
    /// <summary>
    /// Searches through the spawnLocations for points that are within min and max distances from the player and returns a random point.
    /// </summary>
    /// <param name="minRadius">How close the point can be to the player.</param>
    /// <param name="maxRadius">How far the point can be from the player.</param>
    /// <returns>A point as Vector3 if successful, null if not.</returns>
    private Vector3? GetRandomSpawnLocation(List<GameObject> spawnLocationToUse, Vector3 playerPosition, float minRadius = 30f, float maxRadius = 100f)
    {
        List<GameObject> viableSpawnLocations = new List<GameObject>();

        // we sore all possible spawn locations. // ? ToList seems unnecessary, IK before I had a problem but that might been a unity version issue.
        foreach (GameObject possibleSpawnLocation in spawnLocationToUse.ToList())
        {
            if (possibleSpawnLocation == null) continue;

            if (Vector3.Distance(possibleSpawnLocation.transform.position, playerPosition) > minRadius &&
                Vector3.Distance(possibleSpawnLocation.transform.position, playerPosition) < maxRadius)
            {
                viableSpawnLocations.Add(possibleSpawnLocation);
            }
            else
            {
                continue;
            }
        }

        // if we have none, then we return null.
        if (viableSpawnLocations.Count <= 0) return null;

        // pick a random point from the collection.
        return viableSpawnLocations[UnityEngine.Random.Range(0, viableSpawnLocations.Count)].transform.position;
    }
    #endregion


    #region GetEnemyPrefabFromID
    /// <summary>
    /// Get the prefab from the id provided. Searches through aiSpawnList for that id and returns the prefab.
    /// </summary>
    /// <param name="id">The id of the AI in the aiSpawnList.</param>
    /// <returns>Returns the prefab object if successful, null if there wasn't one.</returns>
    private GameObject GetEnemyPrefabFromID(int id)
    {
        foreach (Enemy enemy in enemyCoreData.aISpawnList)
        {
            if (enemy.id == id) return enemy.prefab;
        }

        return null;

    }
    #endregion

    #region RemoveEnemy
    /// <summary>
    /// Removes the enemy that died.
    /// </summary>
    /// <param name="entityTransform">The transform of the enemy that died, may be null.</param>
    private void RemoveEnemy(Transform entityTransform)
    {
        enemyCount--;
        trackingEnemies.Remove(entityTransform);
    }
    #endregion

    #region AddEnemy
    private void AddEnemy(Transform enemyTransform)
    {
        enemyCount++;
        trackingEnemies.Add(enemyTransform);
    }
    #endregion

    #region RandomiseList
    /// <summary>
    /// Util function to randomise a given list.
    /// </summary>
    /// <param name="list">The list to randomise.</param>
    private void RandomiseList(ref List<int> list)
    {

        for (int i = 0; i < list.Count; i++)
        {
            int itemA = list[i];
            int randomLocation = UnityEngine.Random.Range(0, list.Count);

            int itemB = list[randomLocation];
            list[randomLocation] = itemA;

            list[i] = itemB;
        }

    }
    #endregion

    private List<int> ConvertEnemiesInWaveToIDList(EnemiesInWaveData[] enemyInWaveData)
    {
        // I swear there is a func or something built in to create a collection of the same value.
        List<int> listToReturn = new List<int>();
        foreach (EnemiesInWaveData enemy in enemyInWaveData)
        {
            for (int i = 0; i < enemy.amount; i++)
            {
                listToReturn.Add(enemy.id);
            }
        }
        return listToReturn;
    }
}
