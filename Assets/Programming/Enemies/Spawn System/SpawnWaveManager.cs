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
// Licensed for use within the Wraiths of Retail by Smash Keyboard Studios (SKS) only.
// Redistribution or modification outside of this project is prohibited without explicit written permission.
// For full license terms, see DOMIBRON_CODE_LICENSE.md at the project root.





public class SpawnWaveManager : SpawnHandler
{

    public WaveData[] waveData;



    public UnityEvent onWaveEnd;

    public UnityEvent onAllWavesCleared;

    public float minSpawnRadius = 15;
    public float maxSpawnRadius = 40;



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



}
