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




/// <summary>
/// Used to store data in the aiSpawnList, as dictionaries cannot serialize.
/// </summary>
[Serializable]
public struct Enemy
{
	public int id;
	public GameObject prefab;
}

/// <summary>
/// Alternative to using list or dictionary for spawn parameters.
/// </summary>
[Serializable]
public struct EnemyWaveData
{
	public int id;
	public int amount;
}

/// <summary>
/// Singleton class, this does AI wave spawning.
/// </summary>
public class AISpawnSystem : MonoBehaviour
{
	#region Variables



	#region Singleton
	public static AISpawnSystem instance;
	#endregion


	/// <summary>
	/// All possible AI that can be spawned.
	/// </summary>
	[Header("AI spawn list"), SerializeField]
	public Enemy[] aISpawnList;



	private GameObject playerObject;



	#endregion

	/****************************************************************************************/

	#region Functions



	#region Awake
	void Awake()
	{
		if (instance != null && instance != this)
		{
			Destroy(this.gameObject);
		}
		else
		{
			instance = this;
		}

		playerObject = GameObject.Find("Player");
	}
	#endregion



	#region SpawnWave (1)
	/// <summary>
	/// Spawns a wave with the dictionary as spawn sequence. 
	/// The key is the type of ai and the value is the amount.
	/// </summary>
	/// <param name="waveData">Wave spawn parameters. {{0, 4}, {1, 2}} will spawn 4 melee and 2 ranged for example.</param>
	/// <param name="timeBetweenSpawn">How long to wait before spawning the next enemy.</param>
	/// <param name="minRadius">How close the AI can spawn.</param>
	/// <param name="maxRadius">How far the AI can spawn.</param>
	/// <param name="randomCycleAmount">How many times to shuffle the list. (Randomises the list) 0 will disable.</param>
	public void SpawnWave(Dictionary<int, int> waveData, List<GameObject> spawnLocationsToUse, float timeBetweenSpawn = 0.5f, float minRadius = 30f, float maxRadius = 100f, int randomCycleAmount = 2)
	{
		StartCoroutine(SpawnWaveCoroutine(waveData, spawnLocationsToUse, timeBetweenSpawn, minRadius, maxRadius, randomCycleAmount));
	}
	#endregion



	#region SpawnWave (2)
	/// <summary>
	/// Spawns a wave of enemies with the given spawn sequence.
	/// So {0,0,0,0,1,1} will spawn 4 melee and 2 ranged for example.
	/// </summary>
	/// <param name="waveData">Wave spawn parameters. {0,0,0,0,1,1} will spawn 4 melee and 2 ranged for example.</param>
	/// <param name="timeBetweenSpawn">How long to wait before spawning the next enemy.</param>
	/// <param name="minRadius">How close the AI can spawn.</param>
	/// <param name="maxRadius">How far the AI can spawn.</param>
	/// <param name="randomCycleAmount">How many times to shuffle the list. (Randomises the list) 0 will disable.</param>
	public void SpawnWave(int[] waveData, List<GameObject> spawnLocationsToUse, float timeBetweenSpawn = 0.5f, float minRadius = 30f, float maxRadius = 100f, int randomCycleAmount = 2)
	{
		StartCoroutine(SpawnWaveCoroutine(waveData.ToList(), spawnLocationsToUse, timeBetweenSpawn, minRadius, maxRadius, randomCycleAmount));
	}
	#endregion




	#region SpawnWave (4)
	/// <summary>
	/// Spawns a wave of enemies with the given spawn sequence of an array of EnemyWaveData.
	/// EnemyWaveData contains the id and amount, index is ignored.
	/// </summary>
	/// <param name="waveData">The spawn sequence.</param>
	/// <param name="timeBetweenSpawn">How long to wait before spawning the next enemy.</param>
	/// <param name="minRadius">How close the AI can spawn.</param>
	/// <param name="maxRadius">How far the AI can spawn.</param>
	public void SpawnWave(EnemyWaveData[] waveData, List<GameObject> spawnLocationsToUse, float timeBetweenSpawn = 0.5f, float minRadius = 30f, float maxRadius = 100f, int randomCycleAmount = 2)
	{
		Dictionary<int, int> converted = new Dictionary<int, int>();

		for (int i = 0; i < waveData.Length; i++)
		{
			converted.Add(waveData[i].id, waveData[i].amount);
		}

		StartCoroutine(SpawnWaveCoroutine(converted, spawnLocationsToUse, timeBetweenSpawn, minRadius, maxRadius, randomCycleAmount));
	}
	#endregion



	#region SpawnWaveCoroutine
	/// <summary>
	/// Spawns a wave of enemies with the given spawn sequence. Using aiSpawnList, it gets the AI prefab to spawn.
	/// This is dictionary based so {{0, 4}, {1, 2}} will spawn 4 melee and 2 ranged for example. check aiSpawnList for AI id's.
	/// </summary>
	/// <param name="waveData">The spawn sequence for the wave. {{0, 4}, {1, 2}} will spawn 4 melee and 2 ranged for example.</param>
	/// <param name="timeBetweenSpawn">How long to wait before spawning the next enemy.</param>
	/// <param name="minRadius">How close the AI can spawn.</param>
	/// <param name="maxRadius">How far the AI can spawn.</param>
	/// <param name="randomCycleAmount">How many times to shuffle the list. (Randomises the list) 0 will disable.</param>
	/// <returns>Coroutine</returns>
	private IEnumerator SpawnWaveCoroutine(Dictionary<int, int> waveData, List<GameObject> spawnLocationToUse, float timeBetweenSpawn = 0.5f, float minRadius = 30f, float maxRadius = 100f, int randomCycleAmount = 2)
	{
		// convert dictionary into a spawn sequence. This is for randomness.
		// I still want a dictionary for simplicity sake. {0, 30} is 30 melee enemies.
		List<int> spawnSequence = new List<int>();

		foreach (var id in waveData.Keys.ToArray())
		{
			for (int x = 0; x < waveData[id]; x++)
			{
				spawnSequence.Add(id);
			}
		}

		// randomises the list with the given randomCycleAmount, more cycles means the more random the list is.
		for (int i = 0; i < randomCycleAmount; i++)
		{
			RandomiseList(ref spawnSequence);
			yield return null;
		}

		// cycles through the list and spawns the AI with matching id in aISpawnList.
		foreach (int id in spawnSequence.ToList())
		{
			bool successfulSpawn = SpawnAI(id, spawnLocationToUse, minRadius, maxRadius);

			if (!successfulSpawn)
			{
				continue;
			}

			yield return new WaitForSeconds(timeBetweenSpawn);


		}

		yield return null;
	}
	#endregion



	#region SpawnWaveCoroutine
	/// <summary>
	/// Spawns a wave of enemies with the given spawn sequence. Using aiSpawnList, it gets the AI prefab to spawn.
	/// This is list based so {0,0,0,0,1,1} will spawn 4 melee and 2 ranged for example. check aiSpawnList for AI id's.
	/// </summary>
	/// <param name="waveData">The spawn sequence for the wave. {0,0,0,0,1,1} will spawn 4 melee and 2 ranged for example.</param>
	/// <param name="timeBetweenSpawn">How long to wait before spawning the next enemy.</param>
	/// <param name="minRadius">How close the AI can spawn.</param>
	/// <param name="maxRadius">How far the AI can spawn.</param>
	/// <param name="randomCycleAmount">How many times to shuffle the list. (Randomises the list) 0 will disable.</param>
	/// <returns>Coroutine</returns>
	private IEnumerator SpawnWaveCoroutine(List<int> waveData, List<GameObject> spawnLocationToUse, float timeBetweenSpawn = 0.5f, float minRadius = 30f, float maxRadius = 100f, int randomCycleAmount = 2)
	{
		// randomises the list with the given randomCycleAmount, more cycles means the more random the list is.
		for (int i = 0; i < randomCycleAmount; i++)
		{
			RandomiseList(ref waveData);
			yield return null;
		}


		// cycles through the list and spawns the AI with matching id in aISpawnList.
		foreach (int id in waveData.ToList())
		{
			bool successfulSpawn = SpawnAI(id, spawnLocationToUse, minRadius, maxRadius);

			if (!successfulSpawn)
			{
				continue;
			}

			yield return new WaitForSeconds(timeBetweenSpawn);
		}

		yield return null;
	}
	#endregion




	#region SpawnAI
	/// <summary>
	/// Spawn the AI with the given id, min radius to spawn and maxRadius.
	/// </summary>
	/// <param name="id">The id of the ai to look for in aiSpawnList.</param>
	/// <param name="minRadius">How close the AI can spawn.</param>
	/// <param name="maxRadius">How far the AI can spawn.</param>
	/// <returns>True if spawning was successful.</returns>
	private bool SpawnAI(int id, List<GameObject> spawnLocationToUse, float minRadius, float maxRadius)
	{
		// get the AI prefab from the id given.
		GameObject aiPrefab = GetEnemyPrefabFromID(id);

		if (aiPrefab == null)
		{
			Debug.LogWarning($"Prefab missing for ai id of {id}");
			return false;
		}

		// get a random spawn location that is suitable.
		Vector3? spawnLocation = GetRandomSpawnLocation(spawnLocationToUse, minRadius, maxRadius);

		if (!spawnLocation.HasValue)
		{
			Debug.LogWarning($"Cannot find a suitable location to spawn the AI");
			return false;
		}

		// spawn the AI and set state to alerted. This is not proc gen. this is wave gen.
		GameObject ai = Instantiate(aiPrefab, spawnLocation.Value, Quaternion.identity, null);
		ai.GetComponent<AIBase>()?.ChangeState(AIState.Alerted);
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
	private Vector3? GetRandomSpawnLocation(List<GameObject> spawnLocationToUse, float minRadius = 30f, float maxRadius = 100f)
	{
		List<GameObject> viableSpawnLocations = new List<GameObject>();

		// we sore all possible spawn locations.
		foreach (GameObject possibleSpawnLocation in spawnLocationToUse.ToList())
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
		foreach (Enemy enemy in aISpawnList)
		{
			if (enemy.id == id) return enemy.prefab;
		}

		return null;

	}
	#endregion



	#region RandomiseList (2)
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

	#endregion
}
