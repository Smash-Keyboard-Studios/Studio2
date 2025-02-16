using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpawnWaveOnStart : MonoBehaviour
{
	public EnemyWaveData[] waveData;

	IEnumerator Start()
	{
		yield return new WaitForSeconds(1);
		if (AISpawnSystem.singleton != null)
			AISpawnSystem.singleton.SpawnWave(waveData);
	}
}
