using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void SpawnItem(Transform deathLocation)
    {
        if (Random.Range(0f, 100f) <= spawnChance)
            Instantiate(prefabToSpawn, deathLocation.position + spawnOffSetFromEnemy, Quaternion.identity);
    }
}
