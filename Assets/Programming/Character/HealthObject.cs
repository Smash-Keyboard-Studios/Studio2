using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthObject : MonoBehaviour
{
    public float healAmount = 30f;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerStats pStats = other.GetComponent<PlayerStats>();

            if (pStats == null) return;

            if (pStats.PlayerHealth < pStats.PlayerMaxHealth)
            {
                Debug.Log("Healed player");
                pStats.PlayerHealth += healAmount;
                Destroy(gameObject);
            }
        }
    }
}
