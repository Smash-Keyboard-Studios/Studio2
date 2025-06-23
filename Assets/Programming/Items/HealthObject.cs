using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//by    _                 _ _                     
//     | |               (_) |                    
//   __| | ___  _ __ ___  _| |__  _ __ ___  _ __  
//  / _` |/ _ \| '_ ` _ \| | '_ \| '__/ _ \| '_ \ 
// | (_| | (_) | | | | | | | |_) | | | (_) | | | |
//  \__,_|\___/|_| |_| |_|_|_.__/|_|  \___/|_| |_|


public class HealthObject : MonoBehaviour
{
    public float healAmount = 30f;


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        Health playerHealth = other.GetComponent<Health>();

        if (playerHealth == null) return;

        if (playerHealth.GetHealthNormalized() <= 0) return;

        if (playerHealth.GetHealthNormalized() < 1f)
        {
            // Debug.Log("Healed player");

            // AudioManager.Instance.PlayAudio(false, false, other.GetComponent<AudioSource>(), "Plr_Heal");

            playerHealth.AddToHealth(healAmount);

            Destroy(gameObject);
        }


    }
}
