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



public class EnemyHealthPackCarry : MonoBehaviour
{
    public GameObject healthPackToSpawn;

    public Vector3 spawnOffSetFromEnemy = Vector3.zero;

    public GameObject healthPackOnModel;

    [Range(0f, 1f)]
    public float spawnChance = 0.33f;

    // Start is called before the first frame update
    void Start()
    {
        if (Random.Range(0f, 1f) <= spawnChance)
        {
            healthPackOnModel.SetActive(true);
            GetComponent<AIBase>().onDeath += SpawnItem;
        }
        else
        {
            healthPackOnModel.SetActive(false);
        }
    }

    /// <summary>
    /// Spawns the item.
    /// </summary>
    /// <param name="deathLocation">The location of the death.</param>
    private void SpawnItem(Transform deathLocation)
    {

        Instantiate(healthPackToSpawn, deathLocation.position + spawnOffSetFromEnemy, Quaternion.identity);
    }
}
