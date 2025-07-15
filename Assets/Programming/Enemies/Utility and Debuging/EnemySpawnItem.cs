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
/// Spawns the game object after the death of an enemy.
/// </summary>
public class EnemySpawnItem : MonoBehaviour
{
    public GameObject prefabToSpawn;

    public Vector3 spawnOffSetFromEnemy = Vector3.zero;

    [Range(0, 100)]
    public float spawnChance = 33f;

    // Start is called before the first frame update
    void Start()
    {
        // rip
        GetComponent<AIBase>().onDeath += SpawnItem;
    }

    /// <summary>
    /// Spawns the item.
    /// </summary>
    /// <param name="deathLocation">The location of the death.</param>
    private void SpawnItem(Transform deathLocation)
    {
        if (Random.Range(0f, 100f) <= spawnChance)
            Instantiate(prefabToSpawn, deathLocation.position + spawnOffSetFromEnemy, Quaternion.identity);
    }
}
