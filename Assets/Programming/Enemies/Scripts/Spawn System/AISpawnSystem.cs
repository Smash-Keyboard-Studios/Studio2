using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|




[Serializable]
public struct Enemy
{
	public int id;
	public GameObject prefab;
}

[Serializable]
public struct EnemyWaveData
{
	public int id;
	public int amount;
}


public class AISpawnSystem : MonoBehaviour
{
	public static AISpawnSystem singleton;

	/// <summary>
	/// All possible AI that can be spawned.
	/// </summary>
	[SerializeField]
	public Enemy[] aISpawnList;

	/// <summary>
	/// All possible spawn locations for the AI.
	/// </summary>
	public List<GameObject> spawnPoints;

	/// <summary>
	/// Collect all the spawn points 
	/// </summary>
	[SerializeField]
	private bool collectSpawnPointsAutomatically = true;

	/// <summary>
	/// If true, will attempt to update the spawn point array with any new spawn points. Intensive.
	/// </summary>
	[SerializeField]
	private bool constantlyCollectSpawnPointsAutomatically = false;


	private Coroutine collectSpawnLocationsCoroutine;

	private GameObject playerObject;


	void Awake()
	{
		if (singleton != null && singleton != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			singleton = this;
		}

		playerObject = GameObject.Find("Player");
	}

	// Start is called before the first frame update
	void Start()
	{
		if (collectSpawnPointsAutomatically && collectSpawnLocationsCoroutine == null)
		{
			collectSpawnLocationsCoroutine = StartCoroutine(CollectSpawnLocations());
		}
	}



	void FixedUpdate()
	{
		if (collectSpawnPointsAutomatically && constantlyCollectSpawnPointsAutomatically && collectSpawnLocationsCoroutine == null)
		{
			collectSpawnLocationsCoroutine = StartCoroutine(CollectSpawnLocations());
		}
	}



	IEnumerator CollectSpawnLocations()
	{
		GameObject[] possibleSpawnLocations = GameObject.FindGameObjectsWithTag("Spawn Point");

		foreach (GameObject possibleSpawnLocation in possibleSpawnLocations)
		{
			yield return null;
			if (spawnPoints.Contains(possibleSpawnLocation))
			{
				continue;
			}
			else
			{
				spawnPoints.Add(possibleSpawnLocation);
			}
		}

		collectSpawnLocationsCoroutine = null;
	}



	/// <summary>
	/// Spawns a wave with the dictionary as spawn parameters. 
	/// The key is the type of ai and the value is the amount.
	/// </summary>
	/// <param name="waveData">Wave spawn parameters.</param>
	public void SpawnWave(Dictionary<int, int> waveData)
	{
		StartCoroutine(SpawnWaveCoroutine(waveData));
	}



	/// <summary>
	/// Turns the int array into dictionary. key is the type, value is the amount.
	/// Array index is the type and the array value is the amount to spawn.
	/// </summary>
	/// <param name="waveData">Wave spawn parameters.</param>
	public void SpawnWave(int[] waveData)
	{
		Dictionary<int, int> converted = new Dictionary<int, int>();

		for (int i = 0; i < waveData.Length; i++)
		{
			converted.Add(i, waveData[i]);
		}

		StartCoroutine(SpawnWaveCoroutine(converted));
	}



	public void SpawnWave(EnemyWaveData[] waveData)
	{
		Dictionary<int, int> converted = new Dictionary<int, int>();

		for (int i = 0; i < waveData.Length; i++)
		{
			converted.Add(waveData[i].id, waveData[i].amount);
		}

		StartCoroutine(SpawnWaveCoroutine(converted));
	}



	private IEnumerator SpawnWaveCoroutine(Dictionary<int, int> waveData, float timeBetweenSpawn = 0.5f, float minRadius = 30f, float maxRadius = 100f)
	{

		List<int> spawnSequence = new List<int>();

		foreach (var id in waveData.Keys.ToArray())
		{
			for (int x = 0; x < waveData[id]; x++)
			{
				spawnSequence.Add(id);
			}
		}

		RandomiseList(ref spawnSequence, 2);

		foreach (int type in spawnSequence.ToList())
		{
			//if (waveData[type] == 0) continue;

			//for (int i = 0; i < waveData[type]; i++)
			//{
			GameObject aiPrefab = GetEnemyPrefabFromID(type);

			if (aiPrefab == null)
			{
				Debug.LogWarning($"Prefab missing for ai id of {type}");
				continue;
			}

			Vector3? spawnLocation = GetRandomSpawnLocation(minRadius, maxRadius);

			if (!spawnLocation.HasValue)
			{
				Debug.LogWarning($"Cannot find a suitable location to spawn the AI");
				continue;
			}


			GameObject ai = Instantiate(aiPrefab, spawnLocation.Value, Quaternion.identity, null);
			ai.GetComponent<AIBase>()?.ChangeState(AIState.Alerted);

			yield return new WaitForSeconds(timeBetweenSpawn);

			//}
		}

		yield return null;
	}



	private Vector3? GetRandomSpawnLocation(float minRadius = 30f, float maxRadius = 100f)
	{
		List<GameObject> viableSpawnLocations = new List<GameObject>();

		foreach (GameObject possibleSpawnLocation in spawnPoints.ToList())
		{
			if (Vector3.Distance(possibleSpawnLocation.transform.position, playerObject.transform.position) > minRadius &&
				Vector3.Distance(possibleSpawnLocation.transform.position, playerObject.transform.position) < maxRadius)
			{
				viableSpawnLocations.Add(possibleSpawnLocation);
			}
			else
			{
				continue;
			}
		}

		if (viableSpawnLocations.Count <= 0) return null;

		return viableSpawnLocations[UnityEngine.Random.Range(0, viableSpawnLocations.Count)].transform.position;
	}



	private GameObject GetEnemyPrefabFromID(int id)
	{
		foreach (Enemy enemy in aISpawnList)
		{
			if (enemy.id == id) return enemy.prefab;
		}

		return null;

	}

	private void RandomiseList(ref List<int> list, int cycleTimes = 1)
	{
		//List<int> returnedList = new List<int>();
		for (int c = 0; c < cycleTimes; c++)
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
	}
}
