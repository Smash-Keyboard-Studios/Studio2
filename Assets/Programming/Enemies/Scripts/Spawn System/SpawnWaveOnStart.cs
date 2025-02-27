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
/// Spawns a wave of enemies on start with the given values.
/// </summary>
public class SpawnWaveOnStart : MonoBehaviour
{
	/// <summary>
	/// The wave spawn parameters
	/// </summary>
	[Tooltip("The wave spawn sequence")]
	public EnemyWaveData[] waveData;

	[SerializeField, Tooltip("How long to wait before spawning the next enemy in the sequence.")]
	private float timeBetweenSpawn = 0.5f;

	[SerializeField, Tooltip("How close the AI are allowed to spawn.")]
	private float minSpawnRadius = 30f;

	[SerializeField, Tooltip("How far the AI are allowed to spawn.")]
	private float maxSpawnRadius = 100f;

	// ! DO NOT go higher than a few times, this can become expensive to run. Also no big values.
	[SerializeField, Range(0, 20), Tooltip("How random the sequence will be. Set to 0 to disable.")]
	private int sequenceShuffleAmount = 2;

	[SerializeField, Tooltip("All the spawn locations the AI are allow to spawn at")]
	private List<GameObject> spawnPoints;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);
		if (AISpawnSystem.instance != null)
			AISpawnSystem.instance.SpawnWave(waveData, spawnPoints, timeBetweenSpawn, minSpawnRadius, maxSpawnRadius, sequenceShuffleAmount);
	}
}
