using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObject : MonoBehaviour
{
    public float healAmount;
    PlayerStats stats;
    public GameObject player;

    public void Start()
    {
        player.GetComponent<PlayerStats>();
        stats = player.GetComponent<PlayerStats>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (stats.PlayerHealth < stats.PlayerMaxHealth)
            {
                Debug.Log("HEAL ME");
                stats.PlayerHealth += healAmount;
                Destroy(gameObject);
            }
        }
    }
}
